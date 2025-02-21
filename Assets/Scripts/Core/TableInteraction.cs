using UnityEngine;
using UnityEngine.UI;

public class TableInteraction : MonoBehaviour
{
    [SerializeField] private SO_TableBehavior so_TableBehavior;
    [SerializeField] private CurrencyEconomy currencyEconomy;
    [SerializeField] private Button buyBTN;
    [SerializeField] private Button cancelBTN;

    private MeshRenderer meshRenderer;
    private bool isTableLocked = true;
    private static TableInteraction s_current_table;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null && so_TableBehavior != null)
        {
            meshRenderer.material = so_TableBehavior.tableIsLocked ? so_TableBehavior.notPurchased : meshRenderer.material;
            isTableLocked = so_TableBehavior.tableIsLocked;
        }
        buyBTN.onClick.AddListener(OnButtonPressed);
        cancelBTN.onClick.AddListener(OnCancel);
    }

    private void OnMouseDown()
    {
        if (!isTableLocked) return;

        s_current_table = this;
        currencyEconomy.coinsUI.text = so_TableBehavior.costToHire.ToString();
        currencyEconomy.DisplayConfirmPurchase(isTableLocked, gameObject.name);
    }
    void OnButtonPressed()
    {
        if (s_current_table != null && currencyEconomy.CheckAreaPurchase(s_current_table.so_TableBehavior.costToHire))
        {
            s_current_table.isTableLocked = false;
            s_current_table.so_TableBehavior.tableIsLocked = false;
            if (s_current_table.meshRenderer = null)
                s_current_table.meshRenderer.material = s_current_table.so_TableBehavior.purchasedMaterial;

            currencyEconomy.purchaseUI.SetActive(false);
            s_current_table = null;
        }
    }

    void OnCancel()
    {
        currencyEconomy.purchaseUI.SetActive(false);
        s_current_table = null;
    }
}
