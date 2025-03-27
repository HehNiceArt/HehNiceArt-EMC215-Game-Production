using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class TableUpgrade : SerializedMonoBehaviour
{
    public TableInfo tableInfo;
    [HideInInspector] private string tableCategory;
    [HideInInspector] private string tableType;
    [SerializeField] List<TableBehavior> currentTableBehaviors;
    TableInteraction tableInteraction;
    int currentTableLevel = 0;

    [Space(10)]
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
                        tableInteraction.InitializeBehavior(currentTableBehaviors[0]);
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
            currentTableLevel++;

            TableBehavior behaviorToUpdate = currentTableBehaviors[currentTableLevel];

            behaviorToUpdate.UpdateValuesBasedOnLevel();

            Debug.Log($"Before upgrade: {tableInteraction.so_TableBehavior.tableLevels}");

            tableInteraction.InitializeBehavior(behaviorToUpdate);

            Debug.Log($"After upgrade: {tableInteraction.so_TableBehavior.tableLevels}");


            upgradeUI.SetActive(false);
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