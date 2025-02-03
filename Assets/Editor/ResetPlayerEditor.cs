using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ResetPlayerStats))]
public class ResetPlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ResetPlayerStats resetPlayerStats = (ResetPlayerStats)target;
        if (GUILayout.Button("Reset Coins"))
        {
            resetPlayerStats.ResetCoins();
        }
        if (GUILayout.Button("Reset Passive Coin Generation"))
        {
            resetPlayerStats.ResetCoinGen();
        }
        if (GUILayout.Button("Reset Reputaiton"))
        {
            resetPlayerStats.ResetReputation();
        }
        if (GUILayout.Button("Reset Level"))
        {
            resetPlayerStats.ResetLevel();
        }
    }
}
