using TMPro;
using UnityEngine;

public class LevelExperience : MonoBehaviour
{
    [SerializeField] SO_PlayerDetails playerDetails;
    [SerializeField] TextMeshProUGUI levelUI;
    [SerializeField] TextMeshProUGUI reputationUI;
    public float currentXP = 0f;

    void Start()
    {
        UpdateLevel();
        UpdateReputationUI();
    }

    public void AddExperience(float xpAmount)
    {
        currentXP += xpAmount;
        UpdateLevel();
    }

    void UpdateLevel()
    {
        float requiredXP = GetRequiredXPForLevel(playerDetails.playerLevel + 1);

        if (currentXP >= requiredXP)
        {
            playerDetails.playerLevel++;
            currentXP -= requiredXP;
            if (levelUI != null)
                levelUI.text = playerDetails.playerLevel.ToString();
        }
        UpdateReputationUI();
    }

    void UpdateReputationUI()
    {
        if (reputationUI != null)
        {
            string repText;
            if (playerDetails.reputation >= 0)
                repText = $"{playerDetails.reputation:F0}";
            else
                repText = playerDetails.reputation.ToString("F0");
            reputationUI.text = repText;
        }
    }
    public void UpdateReputation(float change)
    {
        playerDetails.reputation += change;
        UpdateReputationUI();
    }

    public float GetRequiredXPForLevel(float level)
    {
        return 100 + (50f * level) + 2 * (level * level);
    }

    public float GetProgressToNextLevel()
    {
        float currentLevel = playerDetails.playerLevel;
        float currentLevelXP = GetRequiredXPForLevel(currentLevel);
        float nextLevelXP = GetRequiredXPForLevel(currentLevel + 1);

        return (currentXP - currentLevelXP) / (nextLevelXP - currentLevelXP);
    }
}
