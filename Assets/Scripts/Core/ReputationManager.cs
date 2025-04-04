using UnityEngine;

public class ReputationManager : MonoBehaviour
{
    [SerializeField] SO_PlayerDetails so_PlayerDetails;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] float[] spawnRates = new float[] { 0, 27, 23, 19, 15, 9, 5, 2 };
    [SerializeField] float[] rateupMultiplier = new float[] { 1, 2, 4, 6 };

    void Update()
    {
        GameOver();
    }
    void GameOver()
    {
        if (so_PlayerDetails.reputation <= -100)
        {
            gameOverUI.SetActive(true);
            Time.timeScale = 0;
        }
    }
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

        if (reputation <= -100)
            return 0; // gameover
        else if (reputation <= -60) // -99 to -60
            return rateupMultiplier[3]; // 6
        else if (reputation <= -30) // -59 to -30
            return rateupMultiplier[2]; // 4
        else if (reputation < 0) // -29 to -1
            return rateupMultiplier[1]; // 2
        else if (reputation <= 30) // 0 to 30
            return rateupMultiplier[0]; // 1
        else if (reputation <= 60) // 31 to 60
            return rateupMultiplier[1]; // 2
        else if (reputation <= 99) // 61 to 99
            return rateupMultiplier[2]; // 4
        else // 100+
            return rateupMultiplier[3]; // 6
    }
}
