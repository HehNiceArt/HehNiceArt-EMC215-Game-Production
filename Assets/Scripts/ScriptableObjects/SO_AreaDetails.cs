using System.Collections.Generic;
using UnityEngine;

public enum WhichArea
{
    firstArea,
    secondArea,
    thirdArea,
    fourthArea,
    fifthArea
}
[CreateAssetMenu(fileName = "SO_AreaDetails", menuName = "Hospital Cats/SO_AreaDetails")]
public class SO_AreaDetails : ScriptableObject
{
    public WhichArea whichArea;
    public string areaName;
    public int slotsPerArea = 3;
    public float playerLevelToUnlock = 1;
    public float costToUnlock = 1000;
    public float incomeRate = 100;
    public bool isLocked = true;
}
