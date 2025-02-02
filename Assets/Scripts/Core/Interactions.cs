using UnityEngine;

[RequireComponent(typeof(PanelStats))]
public class Interactions : MonoBehaviour
{
    public SO_AreaDetails so_AreaDetails;
    [SerializeField] SO_PlayerDetails playerDetails;
    [SerializeField] GameObject areaPanel;
    [SerializeField] PanelStats panelStats;
    public bool isShowing = false;
    private void Start()
    {
        areaPanel.SetActive(false);
    }
    private void OnMouseDown()
    {
        isShowing = !isShowing;
        areaPanel.SetActive(isShowing);
        if (isShowing && panelStats != null)
        {
            panelStats.UpdateStats(so_AreaDetails);
        }
    }
}
