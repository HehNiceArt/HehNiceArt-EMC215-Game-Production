using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(fileName = "SO_TableInfo", menuName = "Hospital Cats/SO_TableInfo")]
public class TableInfo : SerializedMonoBehaviour
{
    public Dictionary<string, List<Dictionary<string, List<SO_TableBehavior>>>> tableBehaviors = new Dictionary<string, List<Dictionary<string, List<SO_TableBehavior>>>>();
}
