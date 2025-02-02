using TMPro;
using UnityEngine;

public class PanelStats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI areaName;
    [SerializeField] TextMeshProUGUI playerLevel;
    [SerializeField] TextMeshProUGUI cost;
    [SerializeField] TextMeshProUGUI incomeRate;
    public void UpdateStats(SO_AreaDetails so_AreaDetails)
    {
        areaName.text = so_AreaDetails.areaName;
        playerLevel.text = so_AreaDetails.playerLevelToUnlock.ToString();
        cost.text = so_AreaDetails.costToUnlock.ToString();
        incomeRate.text = so_AreaDetails.incomeRate.ToString();
    }
}
