using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TableUpgrade : MonoBehaviour
{
    [SerializeField] SO_TableBehavior[] so_TableBehavior;
    TableInteraction tableInteraction;
    public int currentTableLevel = 0;

    [SerializeField] CurrencyEconomy currencyEconomy;
    [SerializeField] Button upgradeBTN;
    [SerializeField] Button cancelBTN;
    [SerializeField] GameObject upgradeUI;
    [SerializeField] TextMeshProUGUI upgradeString;
    [SerializeField] TextMeshProUGUI cost;

    private static TableUpgrade s_tableUpgrade;
    void Start()
    {
        s_tableUpgrade = this;
        tableInteraction = GetComponent<TableInteraction>();
        s_tableUpgrade.upgradeBTN.onClick.AddListener(UpgradeTable);
        s_tableUpgrade.cancelBTN.onClick.AddListener(OnCancel);
    }

    void OnMouseDown()
    {
        s_tableUpgrade = this;
        if (tableInteraction.isTableLocked)
            return;
        if (currentTableLevel == so_TableBehavior.Length - 1)
        {
            Debug.Log("awdaw");
        }
        s_tableUpgrade.upgradeUI.SetActive(true);
        upgradeString.text = $"Upgrade to {so_TableBehavior[currentTableLevel + 1].tableLevels}?";
        cost.text = so_TableBehavior[currentTableLevel + 1].costToHire.ToString();
    }
    public void UpgradeTable()
    {
        s_tableUpgrade = this;
        if (currentTableLevel < s_tableUpgrade.so_TableBehavior.Length - 1 && s_tableUpgrade.currencyEconomy.CheckAreaPurchase(s_tableUpgrade.so_TableBehavior[currentTableLevel + 1].costToHire))
        {
            s_tableUpgrade.tableInteraction.so_TableBehavior = s_tableUpgrade.so_TableBehavior[currentTableLevel + 1];
            s_tableUpgrade.tableInteraction.so_TableBehavior.tableIsLocked = false;
            s_tableUpgrade.upgradeUI.SetActive(false);
            currentTableLevel++;
        }
        else
        {
            upgradeString.text = "Not enough coins!";
        }
    }
    void OnCancel()
    {
        s_tableUpgrade.upgradeUI.SetActive(false);
        s_tableUpgrade = null;
    }
}