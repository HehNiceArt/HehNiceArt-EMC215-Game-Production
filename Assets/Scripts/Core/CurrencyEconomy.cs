using TMPro;
using UnityEngine;

public class CurrencyEconomy : MonoBehaviour
{
    [SerializeField] SO_PlayerDetails so_PlayerDetails;
    public GameObject purchaseUI;
    public TextMeshProUGUI purchaseString;
    public TextMeshProUGUI lvlToUnlockUI;
    public TextMeshProUGUI coinsUI;
    PlayerStats playerStats;

    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        purchaseUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals))
            playerStats.UpdatePlayerDetail(1000f);
        if (Input.GetKeyDown(KeyCode.Minus))
            playerStats.UpdatePlayerDetail(-1000f);

        if (Input.GetKeyDown(KeyCode.LeftBracket))
            playerStats.UpdatePlalerLevel(-1);
        if (Input.GetKeyDown(KeyCode.RightBracket))
            playerStats.UpdatePlalerLevel(1);
    }

    public bool CheckAreaPurchase(float areaCost, float levelCost = 0)
    {
        if (so_PlayerDetails.playerLevel < levelCost)
        {
            purchaseString.text = "Insufficient level!";
            return false;
        }
        if (so_PlayerDetails.coins >= areaCost)
        {
            playerStats.UpdatePlayerDetail(-areaCost);
            return true;
        }
        else
        {
            purchaseString.text = "Not enough coins!";
            return false;
        }
    }

    public void DisplayConfirmPurchase(bool isLocked, string areaName, float lvlToUnlock = 0)
    {
        if (isLocked)
        {
            purchaseUI.SetActive(true);
            purchaseString.text = $"Purchase {areaName}";
            lvlToUnlockUI.gameObject.SetActive(true);
            lvlToUnlockUI.text = lvlToUnlock.ToString();
        }
    }
}