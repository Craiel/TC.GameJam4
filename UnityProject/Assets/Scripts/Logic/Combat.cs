namespace Assets.Scripts.Logic
{
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;
    using Assets.Scripts.Logic.Enums;

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
                ResolveTile(tile, data);
                return;
            }

            var character = data.Target.GetComponent<PlayerCharacterBehavior>();
            if (character != null)
            {
                ResolveCharacter(character, data);
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

        private static void ResolveTile(DestructibleTile tile, CombatResolve data)
        {
            data.Result.Location = tile.transform.position;

            float hit = data.Info.Damage;
            tile.TakeDamage(hit);

            UnityEngine.Debug.Log("TileHit: " + hit);

            data.Result.RegisterHit(data.Info.DamageType, hit);
            Results.Add(data.Result);
        }

        private static void ResolveCharacter(PlayerCharacterBehavior behavior, CombatResolve data)
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
                Results.Add(data.Result);
                return;
            }

            float hit = data.Info.Damage;
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

            /*foreach (a a in a)
            {
                
            }
            character.GetGear()*/

            data.Result.RegisterHit(data.Info.DamageType, hit);
            UnityEngine.Debug.Log(string.Format("Character Hit: {0} -> {1} for {2}", data.Result.SourcePlayerId, data.Result.TargetPlayerId, data.Result.DamageDealtTotal));
            Results.Add(data.Result);
        }

        public static bool CheckPlayerHitMiss(CombatResolve data)
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
