using UnityEngine;
using UnityEditor;

public class ResetAreaEditor : EditorWindow
{
    private ResetAreaStats resetAreaStats;
    [MenuItem("Hospital Cats/Reset Area Stats")]
    public static void ShowWindow()
    {
        GetWindow<ResetAreaEditor>("Reset Area Stats");
    }
    private void OnGUI()
    {
        GUILayout.Label("Reset Area Stats", EditorStyles.boldLabel);
        resetAreaStats = (ResetAreaStats)EditorGUILayout.ObjectField("Reset Area Stats", resetAreaStats, typeof(ResetAreaStats), true);

        if (resetAreaStats == null)
        {
            EditorGUILayout.HelpBox("Assign a ResetAreaStats script", MessageType.Warning);
        }
        if (GUILayout.Button("Reset Locks"))
        {
            resetAreaStats.ResetLocks();
        }
    }
}