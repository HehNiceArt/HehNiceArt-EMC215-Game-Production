using UnityEngine;
using UnityEngine.UI;

public class Interactions : MonoBehaviour
{
    public SO_AreaDetails so_AreaDetails;
    [SerializeField] CurrencyEconomy currencyEconomy;
    [SerializeField] Button buyBTN;
    [SerializeField] Button cancelBTN;
    private static Interactions s_current_interaction;
    static bool isPressed = false;
    [SerializeField] GameObject blackout;
    [SerializeField] Room room;
    [SerializeField] GameObject locks;
    [SerializeField] GameObject purchaseable;

    private void Start()
    {
        room = GetComponent<Room>();
        buyBTN.onClick.AddListener(OnButtonPressed);
        cancelBTN.onClick.AddListener(OnCancel);
        purchaseable.SetActive(false);
    }

    void Update()
    {
        CheckIfPurchaseable();
    }

    void CheckIfPurchaseable()
    {
        if (!so_AreaDetails.isLocked)
        {
            purchaseable.SetActive(false);
            locks.SetActive(false);
        }
        else if (currencyEconomy.so_PlayerDetails.playerLevel >= so_AreaDetails.playerLevelToUnlock)
        {
            locks.SetActive(false);
            purchaseable.SetActive(true);
        }
        else
        {
            locks.SetActive(true);
            purchaseable.SetActive(false);
        }
    }

    private void OnMouseUp()
    {
        if (isPressed) return;
        if (so_AreaDetails.isLocked)
        {
            isPressed = true;
            s_current_interaction = this;
            currencyEconomy.coinsUI.text = so_AreaDetails.costToUnlock.ToString();
            currencyEconomy.lvlToUnlockUI.text = so_AreaDetails.playerLevelToUnlock.ToString();
            currencyEconomy.DisplayConfirmPurchase(so_AreaDetails.isLocked, this.gameObject.name, so_AreaDetails.playerLevelToUnlock);
        }
    }
    void OnButtonPressed()
    {
        if (s_current_interaction != null && currencyEconomy.CheckAreaPurchase(s_current_interaction.so_AreaDetails.costToUnlock, s_current_interaction.so_AreaDetails.playerLevelToUnlock))
        {
            s_current_interaction.so_AreaDetails.isLocked = false;
#pragma warning disable
            FindObjectOfType<LevelExperience>()?.AddExperience(s_current_interaction.so_AreaDetails.xpGain);
            s_current_interaction.blackout.SetActive(false);
            s_current_interaction.room.SetTreatmentTablesActive(true);
            s_current_interaction.locks.SetActive(false);

            s_current_interaction.purchaseable.SetActive(false);

            currencyEconomy.purchaseUI.SetActive(false);
            s_current_interaction = null;
            isPressed = false;
        }
    }
    void OnCancel()
    {
        currencyEconomy.purchaseUI.SetActive(false);
        s_current_interaction = null;
        isPressed = false;
    }
}
