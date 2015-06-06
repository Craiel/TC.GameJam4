namespace Assets.Editor
{
    using System;
    using System.Globalization;

    using Assets.Scripts;
    using Assets.Scripts.Contracts;
    using Assets.Scripts.Logic;

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

        private void AddGearGui(ICharacter actor, GearType type)
        {
            IGear gear = actor.GetGear(type);
            string gearName = gear != null ? gear.Name : "none";
            EditorGUILayout.LabelField(string.Format("{0}: {1}", type, gearName));

            if (GUILayout.Button("Generate Random"))
            {
                PlayerCharacterBehavior characterBehavior = (PlayerCharacterBehavior)this.target;
                characterBehavior.Character.SetGear(type, Systems.GenerateRandomGear(type));
            }

            if (gear == null)
            {
                return;
            }

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(" -- Internal: ");
            foreach (StatType statType in Enum.GetValues(typeof(StatType)))
            {
                float value = gear.GetInternalStat(statType);
                if (Math.Abs(value) > float.Epsilon)
                {
                    EditorGUILayout.TextField(statType.ToString(), value.ToString(CultureInfo.InvariantCulture));
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(" -- Inherited: ");
            foreach (StatType statType in Enum.GetValues(typeof(StatType)))
            {
                float value = gear.GetInheritedStat(statType);
                if (Math.Abs(value) > float.Epsilon)
                {
                    EditorGUILayout.TextField(statType.ToString(), value.ToString(CultureInfo.InvariantCulture));
                }
            }
            EditorGUILayout.EndVertical();
        }
        
        private void BuildCharacterInspectorGui(ICharacter actor)
        {
            this.AddHeaderGuiSection("Stats");
            EditorGUILayout.BeginVertical();
            foreach (StatType type in Enum.GetValues(typeof(StatType)))
            {
                float value = actor.GetStat(type);
                if (Math.Abs(value) > float.Epsilon)
                {
                    EditorGUILayout.TextField(type.ToString(), value.ToString(CultureInfo.InvariantCulture));
                }
            }
            EditorGUILayout.EndVertical();

            this.AddHeaderGuiSection("Gear");

            this.AddGearGui(actor, GearType.Head);
            this.AddGearGui(actor, GearType.Chest);
            this.AddGearGui(actor, GearType.Legs);
            this.AddGearGui(actor, GearType.LeftWeapon);
            this.AddGearGui(actor, GearType.RightWeapon);
        }
    }
}