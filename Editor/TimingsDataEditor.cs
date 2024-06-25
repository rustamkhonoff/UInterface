using UInterface.Extras.EventHandlers;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(TimingsData))]
    public class TimingsDataDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Start the property GUI
            EditorGUI.BeginProperty(position, label, property);

            // Draw the base label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Calculate the positions of the fields
            float labelWidth = EditorGUIUtility.labelWidth / 2 - 10;
            float fieldWidth = (position.width - labelWidth * 2) / 2 - 10;

            Rect durationLabelRect = new Rect(position.x, position.y, labelWidth, position.height);
            Rect durationFieldRect = new Rect(position.x + labelWidth + 5, position.y, fieldWidth, position.height);

            Rect delayLabelRect = new Rect(position.x + labelWidth + fieldWidth + 15, position.y, labelWidth, position.height);
            Rect delayFieldRect = new Rect(position.x + 2 * labelWidth + fieldWidth + 20, position.y, fieldWidth, position.height);

            // Find the SerializedProperties for the fields
            SerializedProperty durationProperty = property.FindPropertyRelative("_duration");
            SerializedProperty delayProperty = property.FindPropertyRelative("_delay");

            // Draw the fields with labels
            EditorGUI.LabelField(durationLabelRect, "Duration");
            EditorGUI.PropertyField(durationFieldRect, durationProperty, GUIContent.none);

            EditorGUI.LabelField(delayLabelRect, "Delay");
            EditorGUI.PropertyField(delayFieldRect, delayProperty, GUIContent.none);

            // End the property GUI
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}