using UnityEngine;
namespace Assets.Scripts.Logic
{
    public static class StatUtils
    {
        public static float CombineStat(StatType type, float first, float second)
        {
            // Todo
            return first + second;
        }

        public static void ApplyDamage(GameObject target, float damage)
        {
            DestructibleTile destructibleTile = target.GetComponent<DestructibleTile>();
            if (destructibleTile != null)
            {
                destructibleTile.TakeDamage(damage);
                return;
            }

            CharacterBehavior characterBehavior = target.GetComponent<CharacterBehavior>();
            if (characterBehavior != null)
            {
                characterBehavior.Character.TakeDamage(damage);
            }
        }
    }
}