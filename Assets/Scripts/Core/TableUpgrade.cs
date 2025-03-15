using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TableUpgrade : MonoBehaviour
{
    public TableInfo tableInfo;
    [SerializeField] private string tableCategory;
    [SerializeField] private string tableType;
    private List<SO_TableBehavior> currentTableBehaviors;
    TableInteraction tableInteraction;
    int currentTableLevel = 0;

    public List<SO_TableBehavior> TableBehaviors => currentTableBehaviors;
    public SO_TableBehavior CurrentBehavior => currentTableBehaviors != null && currentTableLevel < currentTableBehaviors.Count ? currentTableBehaviors[currentTableLevel] : null;

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

#pragma warning disable
        tableInfo = Object.FindObjectOfType<TableInfo>();
        tableCategory = transform.parent.name;
        tableType = transform.name;

        if (tableInfo.tableBehaviors.ContainsKey(tableCategory))
        {
            foreach (var behaviorDict in tableInfo.tableBehaviors[tableCategory])
            {
                if (behaviorDict.ContainsKey(tableType))
                {
                    currentTableBehaviors = behaviorDict[tableType];
                    if (currentTableBehaviors != null && currentTableBehaviors.Count > 0)
                    {
                        tableInteraction.InitializeWithBehavior(currentTableBehaviors[0]);
                    }
                    break;
                }
            }
        }
    }

    void OnMouseUp()
    {
        if (tableInteraction.isTableLocked || currentTableBehaviors == null)
            return;

        if (currentTableLevel == currentTableBehaviors.Count - 1)
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
            upgradeString.text = $"Upgrade to {currentTableBehaviors[currentTableLevel + 1].tableLevels}?";
            cost.text = currentTableBehaviors[currentTableLevel + 1].costToHire.ToString();
        }
    }

    public void UpgradeTable()
    {
        if (currentTableBehaviors == null) return;

        if (currentTableLevel < currentTableBehaviors.Count - 1 && currencyEconomy.CheckAreaPurchase(currentTableBehaviors[currentTableLevel + 1].costToHire))
        {
            float xpGain = currentTableBehaviors[currentTableLevel + 1].xpGain;
#pragma warning disable
            FindObjectOfType<LevelExperience>()?.AddExperience(xpGain);
#pragma warning restore

            tableInteraction.UpdateBehavior(currentTableBehaviors[currentTableLevel + 1]);
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