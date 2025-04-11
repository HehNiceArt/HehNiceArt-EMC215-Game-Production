using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;


[RequireComponent(typeof(TableUpgrade))]
public class TableInteraction : SerializedMonoBehaviour
{
    public TableBehavior so_TableBehavior;
    [SerializeField] private CurrencyEconomy currencyEconomy;
    [SerializeField] private GameObject staff;
    [SerializeField] private Button buyBTN;
    [SerializeField] private Button cancelBTN;
    Interactions interactions;
    float salaryTimer = 0f;
    const float SALARY_INTERVAL = 300;

    public bool isTableLocked = true;
    public bool isOccupied = false;
    private static TableInteraction s_current_table;
    public ParticleSystem coinsParticle;
    [SerializeField] GameObject blackout;

    void Start()
    {
        interactions = GetComponentInParent<Interactions>();
        buyBTN.onClick.AddListener(PurchaseTable);
        cancelBTN.onClick.AddListener(OnCancel);
        if (staff == null) return;
        staff.SetActive(false);
        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag("Blackout"))
            {
                blackout = child.gameObject;
                break;
            }
        }
    }

    public void InitializeBehavior(TableBehavior behavior)
    {
        so_TableBehavior = behavior;
        so_TableBehavior.tableIsLocked = true;
    }
    public void UpdateBehavior(TableBehavior behavior)
    {
        so_TableBehavior = behavior;
        so_TableBehavior.tableIsLocked = false;
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
        Debug.Log("Paid staff salary!");
        FindObjectOfType<PlayerStats>()?.UpdatePlayerDetail(-so_TableBehavior.salary);
    }
    public void OccupyTable() => isOccupied = true;
    public void VacateTable() => isOccupied = false;

    private void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
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
            s_current_table.blackout.SetActive(false);

            if (s_current_table.staff != null)
                s_current_table.staff.SetActive(true);

            FindObjectOfType<LevelExperience>()?.AddExperience(so_TableBehavior.xpGain);
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

