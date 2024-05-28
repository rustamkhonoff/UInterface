using UInterface.Extras.EventHandlers;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(ElementEventHandler), true)]
    public class ElementEventHandlerEditor : UnityEditor.Editor
    {
        private bool showBaseConfiguration = false;
        private bool showTimings = false;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Draw the default inspector for other fields

            // Base Configuration Foldout
            showBaseConfiguration = EditorGUILayout.Foldout(showBaseConfiguration, "Base Configuration");
            if (showBaseConfiguration)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_eventsType"), new GUIContent("Events Type"), true);

                // Timings Foldout within Base Configuration
                showTimings = EditorGUILayout.Foldout(showTimings, "Timings");
                if (showTimings)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_showData"), new GUIContent("Show Data"), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_hideData"), new GUIContent("Hide Data"), true);
                    EditorGUI.indentLevel--;
                }

                EditorGUI.indentLevel--;
            }
            
            DrawPropertiesExcluding(serializedObject, "m_Script", "_eventsType", "_showData", "_hideData");

            serializedObject.ApplyModifiedProperties();
        }
    }
}