using TMPro;
using UnityEngine;

public class PanelStats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI areaName;
    [SerializeField] TextMeshProUGUI playerLevel;
    [SerializeField] TextMeshProUGUI cost;
    [SerializeField] TextMeshProUGUI incomeRate;
    Interactions interactions;
    private void Start()
    {
        interactions = GetComponent<Interactions>();
        SO_AreaDetails so_AreaDetails = interactions.so_AreaDetails;
        areaName.text = so_AreaDetails.areaName;
        playerLevel.text = so_AreaDetails.playerLevelToUnlock.ToString();
        cost.text = so_AreaDetails.costToUnlock.ToString();
        incomeRate.text = so_AreaDetails.incomeRate.ToString();
    }
}
