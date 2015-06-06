namespace Assets.Editor
{
    using System;
    using System.Globalization;

    using Assets.Scripts;
    using Assets.Scripts.Contracts;
    using Assets.Scripts.Logic;
    using Assets.Scripts.Weapons;

    using UnityEditor;

    using UnityEngine;

    [CustomEditor(typeof(PlayerCharacterBehavior))]
    public class CharacterBehaviorInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            if (!Application.isPlaying)
            {
                base.OnInspectorGUI();
                return;
            }

            PlayerCharacterBehavior characterBehavior = (PlayerCharacterBehavior)this.target;
            if (characterBehavior.Character == null)
            {
                EditorGUILayout.LabelField("No Character yet!");
                return;
            }

            this.BuildCharacterInspectorGui(characterBehavior.Character);
        }

        private void AddHeaderGuiSection(string text)
        {
            EditorGUILayout.LabelField("");
            EditorGUILayout.LabelField(string.Format("\t\t ---- {0} ---- \t\t", text));
        }

        private void AddArmorGui(string title, IArmor armor)
        {
            string armorName = armor != null ? armor.Name : "none";
            EditorGUILayout.LabelField(string.Format("{0}: {1}", title, armorName));

            if (GUILayout.Button("Generate Random"))
            {
                // Todo
            }

            if (armor == null)
            {
                return;
            }

            EditorGUILayout.TextField("Armor: ", armor.GetStat(StatType.Armor).ToString(CultureInfo.InvariantCulture));
        }

        private void AddWeaponGui(string title, IWeapon weapon)
        {
            string weaponName = weapon != null ? weapon.Name : "none";
            EditorGUILayout.LabelField(string.Format("{0}: {1}", title, weaponName));

            if (GUILayout.Button("Generate Random"))
            {
                PlayerCharacterBehavior characterBehavior = (PlayerCharacterBehavior)this.target;
                characterBehavior.Character.RightWeapon = new PlainCannon();
                characterBehavior.Character.LeftWeapon = new EnergyCannon();
                // Todo
            }

            if (weapon == null)
            {
                return;
            }

            EditorGUILayout.TextField("Damage: ", weapon.GetStat(StatType.Damage).ToString(CultureInfo.InvariantCulture));
        }

        private void BuildCharacterInspectorGui(ICharacter actor)
        {
            this.AddHeaderGuiSection("Gear");
            
            this.AddArmorGui("HEAD", actor.Head);
            this.AddArmorGui("CHEST", actor.Chest);
            this.AddArmorGui("LEGS", actor.Legs);
            this.AddWeaponGui("LEFT_WEAPON", actor.LeftWeapon);
            this.AddWeaponGui("RIGHT_WEAPON", actor.RightWeapon);

            this.AddHeaderGuiSection("Stats");
            EditorGUILayout.BeginVertical();
            foreach (StatType type in Enum.GetValues(typeof(StatType)))
            {
                float value = actor.GetStat(type);
                EditorGUILayout.TextField(type.ToString(), value.ToString(CultureInfo.InvariantCulture));
            }
            EditorGUILayout.EndVertical();
        }
    }
}