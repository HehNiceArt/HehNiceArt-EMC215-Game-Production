using UnityEngine;

public class ResetPlayerStats : MonoBehaviour
{
    [SerializeField] SO_PlayerDetails sO_PlayerDetails;

    public void ResetCoins()
    {
        sO_PlayerDetails.coins = 99999f; //orig 1250f
    }
    public void ResetReputation()
    {
        sO_PlayerDetails.reputation = 0;
    }
    public void ResetLevel()
    {
        sO_PlayerDetails.playerLevel = 0;
    }
}
