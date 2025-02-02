using UnityEngine;

[RequireComponent(typeof(PanelStats))]
public class Interactions : MonoBehaviour
{
    public SO_AreaDetails so_AreaDetails;
    [SerializeField] GameObject areaPanel;
    public bool isShowing = false;
    private void Start()
    {
        areaPanel.SetActive(false);
    }
    private void OnMouseDown()
    {
        if (!isShowing)
        {
            areaPanel.SetActive(true);
            isShowing = !isShowing;
        }
        else
        {
            areaPanel.SetActive(false);
            isShowing = !isShowing;
        }
    }
}
