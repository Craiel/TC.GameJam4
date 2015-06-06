namespace Assets.Editor
{
    using System;
    using System.Globalization;

    using Assets.Scripts;
    using Assets.Scripts.Contracts;
    using Assets.Scripts.Logic;

    using UnityEditor;

    using UnityEngine;

    [CustomEditor(typeof(CharacterBehavior))]
    public class CharacterBehaviorInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            if (!Application.isPlaying)
            {
                base.OnInspectorGUI();
                return;
            }

            CharacterBehavior characterBehavior = (CharacterBehavior)target;
            if (characterBehavior.Character == null)
            {
                EditorGUILayout.LabelField("No Character yet!");
                return;
            }

            this.BuildCharacterInspectorGui(characterBehavior.Character);
        }

        private void BuildCharacterInspectorGui(IActor actor)
        {
            EditorGUILayout.LabelField("\t\t - -   - - \t\t");
            EditorGUILayout.LabelField("\t\t - Stats - \t\t");
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