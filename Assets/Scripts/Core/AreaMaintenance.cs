using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AreaMaintenance : MonoBehaviour
{
    [SerializeField] private float maintenanceInterval = 600f; // 10 minutes in seconds
    [SerializeField] private float maintenanceDuration = 30f; // 30 seconds
    [SerializeField] private int maintenanceCost = 1500;
    [SerializeField] private List<GameObject> affectedObjects = new List<GameObject>();

    private bool isUnderMaintenance = false;
    private CurrencyEconomy currencyEconomy;

    void Start()
    {
#pragma warning disable
        currencyEconomy = FindObjectOfType<CurrencyEconomy>();
        StartCoroutine(MaintenanceRoutine());

        AreaMaintenance areaMaintenance = FindObjectOfType<AreaMaintenance>();

        GameObject[] tables = GameObject.FindGameObjectsWithTag("table");
        foreach (GameObject table in tables)
        {
            AddAffectedObject(table);
        }
    }

    private IEnumerator MaintenanceRoutine()
    {
        while (true)
        {
            // Wait for maintenance interval
            yield return new WaitForSeconds(maintenanceInterval);

            // Use CheckAreaPurchase instead of CanSpendCoins
            if (currencyEconomy.CheckAreaPurchase(maintenanceCost))
            {
                StartMaintenance();

                // Wait for maintenance duration
                yield return new WaitForSeconds(maintenanceDuration);

                EndMaintenance();
            }
            else
            {
                Debug.LogWarning("Not enough coins for maintenance!");
            }
        }
    }

    private void StartMaintenance()
    {
        isUnderMaintenance = true;

        // Disable all affected objects
        foreach (GameObject obj in affectedObjects)
        {
            obj.SetActive(false);
        }

        // Notify players about maintenance
        Debug.Log("Area maintenance started");
    }

    private void EndMaintenance()
    {
        isUnderMaintenance = false;

        // Re-enable all affected objects
        foreach (GameObject obj in affectedObjects)
        {
            obj.SetActive(true);
        }

        Debug.Log("Area maintenance completed");
    }

    // Method to add objects that should be affected by maintenance
    public void AddAffectedObject(GameObject obj)
    {
        if (!affectedObjects.Contains(obj))
        {
            affectedObjects.Add(obj);
        }
    }

    public bool IsUnderMaintenance()
    {
        return isUnderMaintenance;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
