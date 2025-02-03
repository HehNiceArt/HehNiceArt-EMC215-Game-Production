using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PanelStats))]
public class Interactions : MonoBehaviour
{
    public SO_AreaDetails so_AreaDetails;
    [SerializeField] GameObject areaPanel;
    [SerializeField] CurrencyEconomy currencyEconomy;
    [SerializeField] Button buyBTN;
    [SerializeField] Button cancelBTN;
    PanelStats panelStats;
    public bool isShowing = false;
    private static Interactions s_current_interaction;

    private void Start()
    {
        buyBTN.onClick.AddListener(OnButtonPressed);
        cancelBTN.onClick.AddListener(OnCancel);
        panelStats = GetComponent<PanelStats>();
        areaPanel.SetActive(false);
    }
    private void OnMouseDown()
    {
        isShowing = !isShowing;
        //true if player coins > area lvl to unlock && isLocked
        if (so_AreaDetails.isLocked)
        {
            s_current_interaction = this;
            currencyEconomy.coinsUI.text = so_AreaDetails.costToUnlock.ToString();
            currencyEconomy.DisplayConfirmPurchase(so_AreaDetails.isLocked, this.gameObject.name);
        }
        if (!so_AreaDetails.isLocked)
        {
            areaPanel.SetActive(isShowing);
            panelStats.UpdateStats(so_AreaDetails);
        }
    }
    void OnButtonPressed()
    {
        if (s_current_interaction != null && currencyEconomy.CheckAreaPurchase(s_current_interaction.so_AreaDetails.costToUnlock))
        {
            s_current_interaction.so_AreaDetails.isLocked = false;
            s_current_interaction.areaPanel.SetActive(s_current_interaction.isShowing);
            s_current_interaction.panelStats.UpdateStats(s_current_interaction.so_AreaDetails);
            currencyEconomy.purchaseUI.SetActive(false);
            s_current_interaction = null;
        }
    }
    void OnCancel()
    {
        currencyEconomy.purchaseUI.SetActive(false);
        s_current_interaction = null;
    }
}
