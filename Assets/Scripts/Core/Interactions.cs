using UnityEngine;
using UnityEngine.UI;

public class Interactions : MonoBehaviour
{
    public SO_AreaDetails so_AreaDetails;
    [SerializeField] CurrencyEconomy currencyEconomy;
    [SerializeField] Button buyBTN;
    [SerializeField] Button cancelBTN;
    private static Interactions s_current_interaction;
    [SerializeField] Material changeMat;
    static bool isPressed = false;

    private void Start()
    {
        buyBTN.onClick.AddListener(OnButtonPressed);
        cancelBTN.onClick.AddListener(OnCancel);
    }
    private void OnMouseUp()
    {
        if (isPressed) return;
        if (so_AreaDetails.isLocked)
        {
            isPressed = true;
            s_current_interaction = this;
            currencyEconomy.coinsUI.text = so_AreaDetails.costToUnlock.ToString();
            currencyEconomy.DisplayConfirmPurchase(so_AreaDetails.isLocked, this.gameObject.name);
        }
    }
    void OnButtonPressed()
    {
        if (s_current_interaction != null && currencyEconomy.CheckAreaPurchase(s_current_interaction.so_AreaDetails.costToUnlock))
        {
            s_current_interaction.so_AreaDetails.isLocked = false;
#pragma warning disable
            FindObjectOfType<LevelExperience>()?.AddExperience(s_current_interaction.so_AreaDetails.xpGain);

            MeshRenderer meshRenderer = s_current_interaction.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
                meshRenderer.material = changeMat;
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
