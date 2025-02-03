using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ResetAreaStats))]
public class ResetAreaEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ResetAreaStats resetAreaStats = (ResetAreaStats)target;
        if (GUILayout.Button("Reset Locks"))
        {
            resetAreaStats.ResetLocks();
        }
    }
}