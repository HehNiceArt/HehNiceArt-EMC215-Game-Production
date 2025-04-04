using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Text.RegularExpressions;

public class TableUpgrade : SerializedMonoBehaviour
{
    public TableInfo tableInfo;
    [HideInInspector] private string tableCategory;
    [HideInInspector] private string tableType;
    [SerializeField] List<TableBehavior> currentTableBehaviors;
    [SerializeField] List<GameObject> tableLevelGO;
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

    private static TableUpgrade selectedTable;
    MedStaff_animManager animManager;

    void Start()
    {
        tableInteraction = GetComponent<TableInteraction>();
        animManager = GetComponentInChildren<MedStaff_animManager>();
        upgradeBTN.onClick.AddListener(() =>
        {
            if (selectedTable == this)
                UpgradeTable();
        });
        cancelBTN.onClick.AddListener(OnCancel);

        tableCategory = transform.parent.name;
        tableType = this.name;


        tableInfo = Object.FindAnyObjectByType<TableInfo>();
        InitializeTableBehaviors();
    }

    void InitializeTableBehaviors()
    {
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

        selectedTable = this;
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

            string nextLevel = currentTableBehaviors[currentTableLevel + 1].tableLevels.ToString();
            Match match = Regex.Match(nextLevel, "\"?(Medical_Student|Intern_Doctor|Resident|Fellow|Attending_Physician)\"?");
            string title = match.Success ? match.Groups[1].Value.Replace('_', ' ') : nextLevel.Replace('_', ' ');

            upgradeString.text = $"Upgrade to {title}?";
            cost.text = currentTableBehaviors[currentTableLevel + 1].costToHire.ToString();
        }
    }

    public void UpgradeTable()
    {
        if (currentTableBehaviors == null) return;
        Debug.Log($"Attempting to upgrade table: {tableCategory}/{tableType}");
        Debug.Log($"Current level: {currentTableLevel}, Max level: {currentTableBehaviors.Count - 1}");

        if (currentTableLevel < currentTableBehaviors.Count - 1 && currencyEconomy.CheckAreaPurchase(this.currentTableBehaviors[currentTableLevel + 1].costToHire))
        {
            tableLevelGO[currentTableLevel].SetActive(false);
            currentTableLevel++;
            int lvl = animManager.LVL + 1;
            animManager.SetLevel(lvl);
            tableLevelGO[currentTableLevel].SetActive(true);

            TableBehavior behaviorToUpdate = currentTableBehaviors[currentTableLevel];
            behaviorToUpdate.tableIsLocked = false;
            behaviorToUpdate.UpdateValuesBasedOnLevel();

#pragma warning disable
            FindObjectOfType<LevelExperience>()?.AddExperience(behaviorToUpdate.xpGain);

            Debug.Log($"{tableCategory} {tableType} Before upgrade: {tableInteraction.so_TableBehavior.tableLevels}");

            tableInteraction.InitializeBehavior(behaviorToUpdate);

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
        selectedTable = null;
    }
    void OnDisable()
    {
        if (selectedTable == this)
            selectedTable = null;
    }
}