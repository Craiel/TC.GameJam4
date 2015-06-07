namespace Assets.Scripts.Logic
{
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;
    using Assets.Scripts.Logic.Enums;

    using UnityEngine;

    public static class Combat
    {
        private static readonly IList<CombatResult> Results;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        static Combat()
        {
            Results = new List<CombatResult>();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public static IList<CombatResult> PollResults()
        {
            IList<CombatResult> snapshot = new List<CombatResult>(Results);
            Results.Clear();
            return snapshot;
        }

        public static void Resolve(CombatResolve data)
        {
            UpdateSource(data);

            var tile = data.Target.GetComponent<DestructibleTile>();
            if (tile != null)
            {
                if (ResolveTile(tile, data))
                {
                    Results.Add(data.Result);
                }

                return;
            }

            var character = data.Target.GetComponent<PlayerCharacterBehavior>();
            if (character != null)
            {
                if (ResolveCharacter(character, data))
                {
                    Results.Add(data.Result);
                }

                return;
            }
        }

        private static void UpdateSource(CombatResolve data)
        {
            var characterSource = data.Source.GetComponent<PlayerCharacterBehavior>();
            if (characterSource != null)
            {
                ICharacter character = characterSource.Character;
                System.Diagnostics.Trace.Assert(character != null);
                data.Result.SourcePlayerId = character.Id;
                return;
            }
        }

        private static bool ResolveTile(DestructibleTile tile, CombatResolve data)
        {
            data.Result.Location = tile.transform.position;

            float hit = data.Info.Damage;
            if (!tile.TakeDamage(hit))
            {
                return false;
            }

            UnityEngine.Debug.Log("TileHit: " + hit);

            data.Result.RegisterHit(data.Info.DamageType, hit);
            return true;
        }

        private static bool ResolveCharacter(PlayerCharacterBehavior behavior, CombatResolve data)
        {
            data.Result.Location = behavior.transform.position;

            ICharacter character = behavior.Character;
            System.Diagnostics.Trace.Assert(character != null);

            data.Result.TargetPlayerId = character.Id;

            // Check for miss
            bool isMiss = CheckPlayerHitMiss(data);
            if (isMiss)
            {
                UnityEngine.Debug.Log("Missed player hit");
                data.Result.WasMiss = true;
                return true;
            }

            // Calculate the hit, substract armor and shield based on type
            float hit = data.Info.Damage * data.Info.ModValue;
            hit = Mathf.Log(Mathf.Pow(hit, 2) + 1) * data.Info.LogNMultiplier;
            if (!(hit > 0))
            {
                // Should not happen but we bail out anyway
                return false;
            }

            switch (data.Info.DamageType)
            {
                case DamageType.Projectile:
                    {
                        hit -= character.GetCurrentStat(StatType.Armor);
                        break;
                    }

                case DamageType.Energy:
                    {
                        hit -= character.GetCurrentStat(StatType.Shield);
                        break;
                    }
            }

            if (hit < 0)
            {
                data.Result.WasAbsorbed = true;
                return false;
            }

            bool damageAppliedToGear = ApplyHitToGear(hit, character, data);
            if (damageAppliedToGear)
            {
                UnityEngine.Debug.Log("Applied hit to Gear");
                return true;
            }

            // Check what the type of damage was, some of them are irrelevant beyond this point
            switch (data.Info.DamageType)
            {
                case DamageType.Heat:
                    {
                        return false;
                    }

                case DamageType.Energy:
                case DamageType.Projectile:
                    {
                        // Apply the damage to the character instead
                        character.ModifyStat(StatType.Health, -hit);
                        data.Result.RegisterHit(data.Info.DamageType, hit);
                        UnityEngine.Debug.Log(string.Format("Character Hit: {0} -> {1} for {2}", data.Result.SourcePlayerId, data.Result.TargetPlayerId, data.Result.DamageDealtTotal));
                        return true;
                    }
            }

            return false;
        }

        private static bool ApplyHitToGear(float hit, ICharacter character, CombatResolve data)
        {
            IList<IGear> targetGear = new List<IGear>();
            foreach (GearType gearType in EnumLists.GearTypes)
            {
                IGear gear = character.GetGear(gearType);
                if (gear == null)
                {
                    continue;
                }

                switch (data.Info.DamageType)
                {
                    case DamageType.Energy:
                    case DamageType.Projectile:
                        {
                            float partHealth = gear.GetMaxStat(StatType.Health);
                            float partCurrentHealth = gear.GetCurrentStat(StatType.Health);
                            if (partHealth > 0 && partCurrentHealth > 0)
                            {
                                targetGear.Add(gear);
                            }

                            break;
                        }

                    case DamageType.Heat:
                        {
                            float partHeat = gear.GetMaxStat(StatType.Heat);
                            float partCurrentHeat = gear.GetCurrentStat(StatType.Heat);
                            if (partHeat > 0 && partCurrentHeat < partHeat)
                            {
                                targetGear.Add(gear);
                            }

                            break;
                        }
                }
            }

            if (targetGear.Count > 0)
            {
                IGear gear = targetGear[UnityEngine.Random.Range(0, targetGear.Count)];
                switch (data.Info.DamageType)
                {
                    case DamageType.Energy:
                    case DamageType.Projectile:
                        {
                            gear.ModifyStat(StatType.Health, -hit);
                            data.Result.RegisterHit(data.Info.DamageType, hit);
                            data.Result.WasHitOnGear = true;
                            return true;
                        }

                    case DamageType.Heat:
                        {
                            gear.ModifyStat(StatType.Heat, hit);
                            data.Result.RegisterHit(data.Info.DamageType, hit);
                            data.Result.WasHitOnGear = true;
                            return true;
                        }
                }
            }
                 
            return false;
        }

        private static bool CheckPlayerHitMiss(CombatResolve data)
        {
            var sourceBehavior = data.Source.GetComponent<PlayerCharacterBehavior>();
            if (sourceBehavior != null)
            {
                switch (data.Info.CombatType)
                {
                    case CombatType.Melee:
                        {
                            float accuracy = sourceBehavior.Character.GetCurrentStat(StatType.RangedAccuracy);
                            if (UnityEngine.Random.Range(0f, 1f) < accuracy)
                            {
                                return true;
                            }

                            break;
                        }

                    case CombatType.Ranged:
                        {
                            float accuracy = sourceBehavior.Character.GetCurrentStat(StatType.RangedAccuracy);
                            if (UnityEngine.Random.Range(0f, 1f) < accuracy)
                            {
                                return true;
                            }

                            break;
                        }
                }
            }

            return false;
        }
    }
}
