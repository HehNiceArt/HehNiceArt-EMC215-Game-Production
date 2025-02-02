using UnityEngine;

[CreateAssetMenu(fileName = "SO_PlayerDetails", menuName = "Hospital Cats/SO_PlayerDetails")]
public class SO_PlayerDetails : ScriptableObject
{
    public float coins = 1500f;
    public float passiveCoinGen = 60f;
    public float reputation = 0;
    public float playerLevel = 1;
}
