using TMPro;
using UnityEngine;

public class LevelExperience : MonoBehaviour
{
    [SerializeField] SO_PlayerDetails playerDetails;
    [SerializeField] TextMeshProUGUI levelUI;
    [SerializeField] TextMeshProUGUI reputationUI;
    const float XP_MULTIPLIER = 100f; // This gives us exactly 100 XP for level 1, 400 for level 2, etc.
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
        // Calculate new level using sqrt(XP/100)
        float newLevel = Mathf.Floor(Mathf.Sqrt(currentXP / XP_MULTIPLIER));

        if (newLevel != playerDetails.playerLevel)
        {
            playerDetails.playerLevel = newLevel;
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
                repText = $"+{playerDetails.reputation:F0}";
            else
                repText = playerDetails.reputation.ToString("F0");
            reputationUI.text = repText;

            if (playerDetails.reputation >= SO_PlayerDetails.REPUTATION_TIER_4)
                reputationUI.color = Color.green;
            else if (playerDetails.reputation >= 0)
                reputationUI.color = Color.white;
            else if (playerDetails.reputation > SO_PlayerDetails.MIN_REPUTATION)
                reputationUI.color = new Color(1f, 0.5f, 0.5f); // Light red
            else
                reputationUI.color = Color.red;
        }
    }
    public void UpdateReputation(float change)
    {
        playerDetails.reputation += change;
        UpdateReputationUI();
    }

    // Helper method to calculate required XP for a specific level
    public float GetRequiredXPForLevel(float level)
    {
        return level * level * XP_MULTIPLIER;
    }

    // Helper method to get current XP progress towards next level
    public float GetProgressToNextLevel()
    {
        float currentLevel = playerDetails.playerLevel;
        float currentLevelXP = GetRequiredXPForLevel(currentLevel);
        float nextLevelXP = GetRequiredXPForLevel(currentLevel + 1);

        return (currentXP - currentLevelXP) / (nextLevelXP - currentLevelXP);
    }
}
