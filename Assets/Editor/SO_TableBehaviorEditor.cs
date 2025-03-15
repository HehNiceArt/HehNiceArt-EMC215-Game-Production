using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SO_TableBehaviorEditor : EditorWindow
{
    private Vector2 scrollPosition;
    private List<SO_TableBehavior> tableBehaviors = new List<SO_TableBehavior>();
    private bool showStaffProfiles = false;

    [MenuItem("Hospital Cats/Table Behavior Editor")]
    public static void ShowWindow()
    {
        GetWindow<SO_TableBehaviorEditor>("Table Behavior");
    }

    private void OnEnable()
    {
        RefreshTableBehaviors();
    }

    private void RefreshTableBehaviors()
    {
        string[] guids = AssetDatabase.FindAssets("t:SO_TableBehavior");
        tableBehaviors.Clear();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            SO_TableBehavior asset = AssetDatabase.LoadAssetAtPath<SO_TableBehavior>(path);
            if (asset != null)
            {
                tableBehaviors.Add(asset);
            }
        }
    }

    private void ResetAllTableLocks()
    {
        Undo.RecordObjects(tableBehaviors.ToArray(), "Reset All Table Locks");

        foreach (SO_TableBehavior table in tableBehaviors)
        {
            table.tableIsLocked = true;
            EditorUtility.SetDirty(table);
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"Reset {tableBehaviors.Count} tables to locked state");
    }

    private void OnGUI()
    {
        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Refresh Table Behaviors"))
        {
            RefreshTableBehaviors();
        }

        GUI.backgroundColor = Color.yellow;
        if (GUILayout.Button("Reset All Tables Lock Status"))
        {
            if (EditorUtility.DisplayDialog("Reset Table Locks",
                "Are you sure you want to reset all tables to locked state?",
                "Yes", "No"))
            {
                ResetAllTableLocks();
            }
        }
        GUI.backgroundColor = Color.white;
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Available Table Behaviors", EditorStyles.boldLabel);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        foreach (SO_TableBehavior table in tableBehaviors)
        {
            EditorGUILayout.BeginVertical("box");

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.ObjectField("Table Asset", table, typeof(SO_TableBehavior), false);
            EditorGUILayout.EnumPopup("Table Level", table.tableLevels);

            EditorGUILayout.FloatField("Treatment Cost", table.treatmentCost);
            EditorGUILayout.FloatField("Salary", table.salary);
            EditorGUILayout.FloatField("Cost to Hire", table.costToHire);
            EditorGUILayout.FloatField("XP Gain", table.xpGain);
            EditorGUILayout.ObjectField("Not Purchased Material", table.notPurchased, typeof(Material), false);
            EditorGUILayout.ObjectField("Purchased Material", table.purchasedMaterial, typeof(Material), false);
            EditorGUILayout.Toggle("Table Is Locked", table.tableIsLocked);

            EditorGUILayout.EndVertical();
            GUILayout.Space(10);
        }

        EditorGUILayout.EndScrollView();
    }
}