using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TableUpgrade : MonoBehaviour
{
    [SerializeField] SO_TableBehavior[] so_TableBehavior;
    TableInteraction tableInteraction;
    private int currentTableLevel = 0;

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
        upgradeBTN.onClick.AddListener(UpgradeTable);
        cancelBTN.onClick.AddListener(OnCancel);
    }

    void OnMouseDown()
    {
        if (tableInteraction.isTableLocked)
            return;
        s_tableUpgrade = this;
        s_tableUpgrade.upgradeUI.SetActive(true);
        upgradeString.text = $"Upgrade to {so_TableBehavior[currentTableLevel + 1].tableLevels}?";
        cost.text = so_TableBehavior[currentTableLevel + 1].costToHire.ToString();
    }
    public void UpgradeTable()
    {
        if (currentTableLevel < s_tableUpgrade.so_TableBehavior.Length - 1 && currencyEconomy.CheckAreaPurchase(s_tableUpgrade.so_TableBehavior[currentTableLevel + 1].costToHire))
        {
            Debug.Log("Upgrade table!");
            s_tableUpgrade.tableInteraction.so_TableBehavior = s_tableUpgrade.so_TableBehavior[currentTableLevel + 1];
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