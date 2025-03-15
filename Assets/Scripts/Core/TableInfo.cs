using System;
using System.Collections.Generic;
using UnityEngine;

public class TableInfo : MonoBehaviour
{
    [SerializeField]
    TableInfoDictionary[] tableInfoDictionaries;

}

[Serializable]
public class TableInfoDictionary
{
    [SerializeField]
    string areaTable;
    [SerializeField]
    List<SO_TableBehavior> tableBehaviors;
}