namespace Assets.Scripts.Logic
{
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;

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

            data.Result.RegisterHit(data.Info.Type, hit);
            Results.Add(data.Result);
        }

        private static void ResolveCharacter(PlayerCharacterBehavior behavior, CombatResolve data)
        {
            data.Result.Location = behavior.transform.position;

            ICharacter character = behavior.Character;
            System.Diagnostics.Trace.Assert(character != null);

            data.Result.TargetPlayerId = character.Id;
            /*switch (data.Info.Type)
            {
                    case DamageType.Energy:
                    {
                        character
                    }
            }*/
        }
    }
}
