using UnityEngine;

public enum StationCost
{
    firstStation,
    secondStation,
    thirdStation,
    fourthStation,
    fifthStation
}
[CreateAssetMenu(fileName = "SO_TableDetails", menuName = "Hospital Cats/SO_TableDetails")]
public class SO_TableDetails : ScriptableObject
{
    public StationCost stationCost;
    public GameObject[] stationPrefabs;
}
