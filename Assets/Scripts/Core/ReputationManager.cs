using UnityEngine;

public class ReputationManager : MonoBehaviour
{
    [SerializeField] SO_PlayerDetails so_PlayerDetails;
    [SerializeField] float[] spawnRates = new float[] { 0, 27, 23, 19, 15, 9, 5, 2 };
    [SerializeField] float[] rateupRates = new float[] { 0, 2, 4, 6 };

    public float GetPatientSpawnRate()
    {
        float reputation = so_PlayerDetails.reputation;

        if (reputation <= -100f)
            return spawnRates[0];
        else if (reputation <= -60f)
            return spawnRates[1];
        else if (reputation <= -30f)
            return spawnRates[2];
        else if (reputation < 0f)
            return spawnRates[3];
        else if (reputation <= 30f)
            return spawnRates[4];
        else if (reputation <= 60f)
            return spawnRates[5];
        else if (reputation <= 99f)
            return spawnRates[6];
        else
            return spawnRates[7];
    }

    public float RateupMultiplier()
    {
        float reputation = so_PlayerDetails.reputation;

        if (reputation <= -100f)
            return rateupRates[0];
        else if (reputation <= -60f)
            return rateupRates[3];
        else if (reputation <= -30f)
            return rateupRates[2];
        else if (reputation < 0f)
            return rateupRates[1];
        else if (reputation <= 30f)
            return 1;
        else if (reputation <= 60f)
            return rateupRates[1];
        else if (reputation <= 99f)
            return rateupRates[2];
        else
            return rateupRates[3];
    }
}
