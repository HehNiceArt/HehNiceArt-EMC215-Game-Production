using UnityEngine;
using UnityEngine.UI;

public class TableInteraction : MonoBehaviour
{
    public SO_TableBehavior so_TableBehavior;
    [SerializeField] private CurrencyEconomy currencyEconomy;
    [SerializeField] private Button buyBTN;
    [SerializeField] private Button cancelBTN;
    Interactions interactions;

    private MeshRenderer meshRenderer;
    public bool isTableLocked = true;
    public bool isOccupied = false;
    private static TableInteraction s_current_table;

    void Start()
    {
        interactions = GetComponentInParent<Interactions>();
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null && so_TableBehavior != null)
        {
            meshRenderer.material = so_TableBehavior.tableIsLocked ? so_TableBehavior.notPurchased : meshRenderer.material;
            isTableLocked = so_TableBehavior.tableIsLocked;
        }
        buyBTN.onClick.AddListener(PurchaseTable);
        cancelBTN.onClick.AddListener(OnCancel);
    }
    public void OccupyTable() => isOccupied = true;
    public void VacateTable() => isOccupied = false;

    private void OnMouseDown()
    {
        if (!isTableLocked) return;
        if (interactions.so_AreaDetails.isLocked) return;

        s_current_table = this;
        currencyEconomy.coinsUI.text = so_TableBehavior.costToHire.ToString();
        currencyEconomy.DisplayConfirmPurchase(isTableLocked, gameObject.name);
    }
    void PurchaseTable()
    {
        if (s_current_table != null && currencyEconomy.CheckAreaPurchase(s_current_table.so_TableBehavior.costToHire))
        {
            s_current_table.isTableLocked = false;
            s_current_table.so_TableBehavior.tableIsLocked = false;
            s_current_table.meshRenderer.material = s_current_table.so_TableBehavior.purchasedMaterial;

#pragma warning disable
            FindObjectOfType<LevelExperience>()?.AddExperience(so_TableBehavior.xpGain);
#pragma warning restore
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
