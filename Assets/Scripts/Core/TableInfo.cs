using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class TableInfo : SerializedMonoBehaviour
{
    public Dictionary<string, List<Dictionary<string, List<TableBehavior>>>> tableBehaviors = new Dictionary<string, List<Dictionary<string, List<TableBehavior>>>>();

    private void OnValidate()
    {
        if (tableBehaviors == null) return;

        foreach (var level1 in tableBehaviors.Values)
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
                            behavior.UpdateValuesBasedOnLevel();
                        }
                    }
                }
            }
        }
    }
}
