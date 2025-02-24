using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TableUpgrade : MonoBehaviour
{
    [SerializeField] SO_TableBehavior[] so_TableBehavior;
    TableInteraction tableInteraction;
    int currentTableLevel = 0;

    [SerializeField] CurrencyEconomy currencyEconomy;
    [SerializeField] Button upgradeBTN;
    [SerializeField] Button cancelBTN;
    public GameObject upgradeUI;
    [SerializeField] TextMeshProUGUI upgradeString;
    [SerializeField] TextMeshProUGUI cost;
    [SerializeField] TextMeshProUGUI costText;

    void Start()
    {
        tableInteraction = GetComponent<TableInteraction>();
        upgradeBTN.onClick.AddListener(UpgradeTable);
        cancelBTN.onClick.AddListener(OnCancel);
    }

    void OnMouseUp()
    {
        if (tableInteraction.isTableLocked)
            return;
        if (currentTableLevel == so_TableBehavior.Length - 1)
        {
            upgradeUI.SetActive(true);
            upgradeString.text = "Max Level!";
            upgradeBTN.gameObject.SetActive(false);
            costText.gameObject.SetActive(false);
            cost.gameObject.SetActive(false);
            return;
        }
        else
        {
            upgradeUI.SetActive(true);
            upgradeBTN.gameObject.SetActive(true);
            costText.gameObject.SetActive(true);
            cost.gameObject.SetActive(true);
            upgradeString.text = $"Upgrade to {so_TableBehavior[currentTableLevel + 1].tableLevels}?";
            cost.text = so_TableBehavior[currentTableLevel + 1].costToHire.ToString();
        }
    }
    public void UpgradeTable()
    {
        if (currentTableLevel < so_TableBehavior.Length - 1 && currencyEconomy.CheckAreaPurchase(so_TableBehavior[currentTableLevel + 1].costToHire))
        {
            float xpGain = so_TableBehavior[currentTableLevel + 1].xpGain;
#pragma warning disable
            FindObjectOfType<LevelExperience>()?.AddExperience(xpGain);
#pragma warning restore

            tableInteraction.so_TableBehavior = so_TableBehavior[currentTableLevel + 1];
            tableInteraction.so_TableBehavior.tableIsLocked = false;
            upgradeUI.SetActive(false);
            currentTableLevel++;
        }
        else
        {
            upgradeString.text = "Not enough coins!";
        }
    }
    void OnCancel()
    {
        upgradeUI.SetActive(false);
    }
}