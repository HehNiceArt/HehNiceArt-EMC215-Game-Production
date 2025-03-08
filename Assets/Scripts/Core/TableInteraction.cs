using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TableUpgrade))]
public class TableInteraction : MonoBehaviour
{
    [HideInInspector] public SO_TableBehavior so_TableBehavior;
    [SerializeField] private CurrencyEconomy currencyEconomy;
    [SerializeField] private Button buyBTN;
    [SerializeField] private Button cancelBTN;
    Interactions interactions;
    float salaryTimer = 0f;
    const float SALARY_INTERVAL = 300f;

    private MeshRenderer meshRenderer;
    public bool isTableLocked = true;
    public bool isOccupied = false;
    private static TableInteraction s_current_table;
    TableUpgrade tableUpgrade;

    void Start()
    {
        tableUpgrade = GetComponent<TableUpgrade>();
        interactions = GetComponentInParent<Interactions>();
        meshRenderer = GetComponent<MeshRenderer>();
        so_TableBehavior = tableUpgrade.so_TableBehavior[0];
        if (meshRenderer != null && so_TableBehavior != null)
        {
            meshRenderer.material = so_TableBehavior.tableIsLocked ? so_TableBehavior.notPurchased : meshRenderer.material;
            isTableLocked = so_TableBehavior.tableIsLocked;
        }
        buyBTN.onClick.AddListener(PurchaseTable);
        cancelBTN.onClick.AddListener(OnCancel);
    }
    void Update()
    {
        if (!isTableLocked)
        {
            salaryTimer += Time.deltaTime;
            if (salaryTimer >= SALARY_INTERVAL)
            {
                PayStaffSalary();
                salaryTimer = 0;
            }
        }
    }
#pragma warning disable
    void PayStaffSalary()
    {
        FindObjectOfType<PlayerStats>()?.UpdatePlayerDetail(-so_TableBehavior.salary);
    }
    public void OccupyTable() => isOccupied = true;
    public void VacateTable() => isOccupied = false;

    private void OnMouseUp()
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
