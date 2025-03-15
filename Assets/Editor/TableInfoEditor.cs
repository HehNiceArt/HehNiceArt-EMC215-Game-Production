using UnityEngine;
using UnityEditor;
using System.Linq;

public class TableInfoEditor : EditorWindow
{
    private TableInfo selectedTableInfo;
    private Vector2 scrollPosition;

    [MenuItem("Hospital Cats/Table Info Manager")]
    public static void ShowWindow()
    {
        GetWindow<TableInfoEditor>("Table Info Manager");
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Table Info Manager", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        selectedTableInfo = (TableInfo)EditorGUILayout.ObjectField("Table Info", selectedTableInfo, typeof(TableInfo), true);

        if (selectedTableInfo == null)
        {
            EditorGUILayout.HelpBox("Please select a TableInfo component", MessageType.Info);
            return;
        }

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Lock All Tables"))
        {
            LockAllTables();
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("All Table Behaviors", EditorStyles.boldLabel);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        DisplayTableBehaviors();
        EditorGUILayout.EndScrollView();
    }

    private void LockAllTables()
    {
        if (selectedTableInfo == null || selectedTableInfo.tableBehaviors == null)
            return;

        Undo.RecordObject(selectedTableInfo, "Lock All Tables");

        foreach (var level1 in selectedTableInfo.tableBehaviors.Values)
        {
            if (level1 == null) continue;

            foreach (var level2 in level1)
            {
                if (level2 == null) continue;

                foreach (var behaviors in level2.Values)
                {
                    if (behaviors == null) continue;

                    foreach (var behavior in behaviors)
                    {
                        if (behavior != null)
                        {
                            behavior.tableIsLocked = true;
                        }
                    }
                }
            }
        }

        EditorUtility.SetDirty(selectedTableInfo);
    }

    private void DisplayTableBehaviors()
    {
        if (selectedTableInfo == null || selectedTableInfo.tableBehaviors == null)
            return;

        foreach (var level1Entry in selectedTableInfo.tableBehaviors)
        {
            EditorGUILayout.LabelField($"Category: {level1Entry.Key}", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            foreach (var level2Dict in level1Entry.Value)
            {
                foreach (var level2Entry in level2Dict)
                {
                    EditorGUILayout.LabelField($"Type: {level2Entry.Key}");
                    EditorGUI.indentLevel++;

                    foreach (var behavior in level2Entry.Value)
                    {
                        if (behavior != null)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField($"Level: {behavior.tableLevels}");
                            behavior.tableIsLocked = EditorGUILayout.Toggle("Is Locked", behavior.tableIsLocked);
                            EditorGUILayout.EndHorizontal();
                        }
                    }

                    EditorGUI.indentLevel--;
                }
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.Space(5);
        }
    }
}