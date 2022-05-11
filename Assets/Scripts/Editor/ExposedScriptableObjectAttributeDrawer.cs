using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ExposedScriptableObjectAttribute))]
public class ExposedScriptableObjectAttributeDrawer : PropertyDrawer
{
    Editor editor = null;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, label, true);

        if (property.objectReferenceValue != null)
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;
            if (!editor)
                Editor.CreateCachedEditor(property.objectReferenceValue, null, ref editor);

            EditorGUI.BeginChangeCheck();
            if (editor)
                editor.OnInspectorGUI();
            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.indentLevel--;
        }
    }
}
