using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Interactions : MonoBehaviour
{
    public SO_AreaDetails so_AreaDetails;
    [SerializeField] CurrencyEconomy currencyEconomy;
    [SerializeField] Button buyBTN;
    [SerializeField] Button cancelBTN;
    private static Interactions s_current_interaction;
    [SerializeField] Material changeMat;
    static bool isPressed = false;
    [SerializeField] GameObject[] tableSpawnPoints;

    private void Start()
    {
        tableSpawnPoints = GetChildTables(this.gameObject);
        buyBTN.onClick.AddListener(OnButtonPressed);
        cancelBTN.onClick.AddListener(OnCancel);
    }
    GameObject[] GetChildTables(GameObject parent)
    {
        Transform[] all = parent.GetComponentsInChildren<Transform>();
        GameObject[] child = System.Array.FindAll(all, t => t != parent.transform).Select(t => t.gameObject).ToArray();

        return child;
    }
    private void OnMouseDown()
    {
        if (isPressed) return;
        if (so_AreaDetails.isLocked)
        {
            isPressed = true;
            s_current_interaction = this;
            currencyEconomy.coinsUI.text = so_AreaDetails.costToUnlock.ToString();
            currencyEconomy.DisplayConfirmPurchase(so_AreaDetails.isLocked, this.gameObject.name);
        }
    }
    void OnButtonPressed()
    {
        if (s_current_interaction != null && currencyEconomy.CheckAreaPurchase(s_current_interaction.so_AreaDetails.costToUnlock))
        {
            s_current_interaction.so_AreaDetails.isLocked = false;

            MeshRenderer meshRenderer = s_current_interaction.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
                meshRenderer.material = changeMat;

            foreach (GameObject tableSpawnPoint in tableSpawnPoints)
            {
                if (so_AreaDetails.stations != null && so_AreaDetails.stations.Length > 0)
                {
                    GameObject stationPrefab = so_AreaDetails.stations[0];
                    GameObject table = Instantiate(stationPrefab, tableSpawnPoint.transform.position, tableSpawnPoint.transform.rotation);
                    table.transform.parent = tableSpawnPoint.transform;
                }
            }
            currencyEconomy.purchaseUI.SetActive(false);
            s_current_interaction = null;
            isPressed = false;
        }
    }
    void OnCancel()
    {
        currencyEconomy.purchaseUI.SetActive(false);
        s_current_interaction = null;
        isPressed = false;
    }
}
