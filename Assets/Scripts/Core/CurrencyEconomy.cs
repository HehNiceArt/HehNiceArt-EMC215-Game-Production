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
    }

    public bool CheckAreaPurchase(float areaCost)
    {
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
            lvlToUnlockUI.text = lvlToUnlock.ToString();
        }
    }
}