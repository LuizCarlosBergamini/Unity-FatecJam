using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Dialog_SO))]
public class Dialog_SOEditor : Editor
{
    private SerializedProperty entitiesProp;
    private SerializedProperty dialogsProp;

    private void OnEnable()
    {
        entitiesProp = serializedObject.FindProperty("entities");
        dialogsProp = serializedObject.FindProperty("dialogs");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // --- ENTITIES ---
        EditorGUILayout.LabelField("Entities", EditorStyles.boldLabel);
        for (int i = 0; i < entitiesProp.arraySize; i++)
        {
            SerializedProperty entityProp = entitiesProp.GetArrayElementAtIndex(i);
            SerializedProperty nameProp = entityProp.FindPropertyRelative("name");
            SerializedProperty iconProp = entityProp.FindPropertyRelative("icon");

            EditorGUILayout.BeginHorizontal();

            // Nome só leitura
            EditorGUILayout.LabelField(nameProp.stringValue, GUILayout.Width(120));

            // Ícone editável
            EditorGUILayout.PropertyField(iconProp, GUIContent.none, GUILayout.Width(100));

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space(10);

        // --- DIALOGS ---
        EditorGUILayout.LabelField("Dialogs", EditorStyles.boldLabel);

        for (int i = 0; i < dialogsProp.arraySize; i++)
        {
            SerializedProperty dialogProp = dialogsProp.GetArrayElementAtIndex(i);
            SerializedProperty entityProp = dialogProp.FindPropertyRelative("entity");
            SerializedProperty textProp = dialogProp.FindPropertyRelative("text");
            SerializedProperty durationProp = dialogProp.FindPropertyRelative("duration");

            SerializedProperty dialogEntityName = entityProp.FindPropertyRelative("name");
            SerializedProperty dialogEntityIcon = entityProp.FindPropertyRelative("icon");

            EditorGUILayout.BeginVertical("box");

            // Dropdown de entidades
            List<string> entityNames = new List<string>();
            for (int j = 0; j < entitiesProp.arraySize; j++)
            {
                entityNames.Add(entitiesProp.GetArrayElementAtIndex(j).FindPropertyRelative("name").stringValue);
            }

            int currentIndex = Mathf.Max(0, entityNames.IndexOf(dialogEntityName.stringValue));
            int newIndex = EditorGUILayout.Popup("Entity", currentIndex, entityNames.ToArray());

            if (newIndex >= 0 && newIndex < entitiesProp.arraySize)
            {
                SerializedProperty selectedEntity = entitiesProp.GetArrayElementAtIndex(newIndex);
                dialogEntityName.stringValue = selectedEntity.FindPropertyRelative("name").stringValue;
                dialogEntityIcon.objectReferenceValue = selectedEntity.FindPropertyRelative("icon").objectReferenceValue;
            }

            // Texto e duração
            textProp.stringValue = EditorGUILayout.TextArea(textProp.stringValue, GUILayout.MinHeight(60));
            durationProp.floatValue = EditorGUILayout.FloatField("Duration", durationProp.floatValue);

            if (GUILayout.Button("Remove Dialog"))
            {
                dialogsProp.DeleteArrayElementAtIndex(i);
                break;
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(2);
        }

        if (GUILayout.Button("Add Dialog"))
        {
            dialogsProp.InsertArrayElementAtIndex(dialogsProp.arraySize);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
