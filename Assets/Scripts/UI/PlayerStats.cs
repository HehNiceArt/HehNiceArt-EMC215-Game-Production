using TMPro;
using UnityEngine;

[RequireComponent(typeof(CurrencyEconomy))]
public class PlayerStats : MonoBehaviour
{
    [SerializeField] SO_PlayerDetails so_PlayerDetails;
    [SerializeField] TextMeshProUGUI coinsUI;
    [SerializeField] TextMeshProUGUI levelUI;

    private void Start()
    {
        coinsUI.text = so_PlayerDetails.coins.ToString();
        levelUI.text = so_PlayerDetails.playerLevel.ToString();
    }

    public void UpdatePlayerDetail(float updateCoins)
    {
        so_PlayerDetails.coins += updateCoins;
        coinsUI.text = so_PlayerDetails.coins.ToString();
    }
}
