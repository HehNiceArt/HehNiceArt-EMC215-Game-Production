using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TableUpgrade : MonoBehaviour
{
    [SerializeField] private SO_TableBehavior so_TableBehavior;
    [SerializeField] private CurrencyEconomy currencyEconomy;
    [SerializeField] private Button upgradeBTN;
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private TextMeshProUGUI currentLevelText;

    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        upgradeBTN.onClick.AddListener(OnUpgradeButtonPressed);
        UpdateUI();
    }

    public void SetupReferences(SO_TableBehavior tableBehavior, CurrencyEconomy economy)
    {
        so_TableBehavior = tableBehavior;
        currencyEconomy = economy;
    }

    private void UpdateUI()
    {
        if (so_TableBehavior != null)
        {
            upgradeCostText.text = CalculateUpgradeCost().ToString();
            currentLevelText.text = $"Current Level: {so_TableBehavior.tableLevels}";
        }
    }

    private float CalculateUpgradeCost()
    {
        // Increase cost based on current level
        return so_TableBehavior.costToHire * 1.5f;
    }

    private void OnUpgradeButtonPressed()
    {
        float upgradeCost = CalculateUpgradeCost();

        if (currencyEconomy.CheckAreaPurchase(upgradeCost))
        {
            UpgradeTable();
            UpdateUI();
        }
    }

    private void UpgradeTable()
    {
        switch (so_TableBehavior.tableLevels)
        {
            case TableLevels.medicalStudent:
                so_TableBehavior.tableLevels = TableLevels.internDoctor;
                break;
            case TableLevels.internDoctor:
                so_TableBehavior.tableLevels = TableLevels.resident;
                break;
            case TableLevels.resident:
                so_TableBehavior.tableLevels = TableLevels.fellow;
                break;
            case TableLevels.fellow:
                so_TableBehavior.tableLevels = TableLevels.attendingPhysician;
                break;
        }

        // Update table properties
        so_TableBehavior.treatmentCost *= 1.5f;
        so_TableBehavior.salary *= 1.25f;
        so_TableBehavior.costToHire *= 2f;

        // Update visual if needed
        if (meshRenderer != null && so_TableBehavior.staffProfile.Length > (int)so_TableBehavior.tableLevels)
        {
            meshRenderer.material.mainTexture = so_TableBehavior.staffProfile[(int)so_TableBehavior.tableLevels];
        }
    }
}