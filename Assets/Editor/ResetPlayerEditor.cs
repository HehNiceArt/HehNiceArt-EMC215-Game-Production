using UnityEngine;
using UnityEditor;

public class ResetPlayerEditor : EditorWindow
{
    private ResetPlayerStats resetPlayerStats;
    [MenuItem("Hospital Cats/Reset Player Stats")]
    public static void ShowWindow()
    {
        GetWindow<ResetPlayerEditor>("Reset Player Stats");
    }

    private void OnGUI()
    {
        GUILayout.Label("Reset Player Stats", EditorStyles.boldLabel);
        resetPlayerStats = (ResetPlayerStats)EditorGUILayout.ObjectField("Reset Player Stats", resetPlayerStats, typeof(ResetPlayerStats), true);

        if (resetPlayerStats == null)
        {
            EditorGUILayout.HelpBox("Assign a ResetPlayerStats script", MessageType.Warning);
        }
        if (GUILayout.Button("Reset Coins"))
        {
            resetPlayerStats.ResetCoins();
        }
        if (GUILayout.Button("Reset Reputation"))
        {
            resetPlayerStats.ResetReputation();
        }
        if (GUILayout.Button("Reset Level"))
        {
            resetPlayerStats.ResetLevel();
        }

    }
}
