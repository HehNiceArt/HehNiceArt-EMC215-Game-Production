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
            yield return new WaitForSeconds(maintenanceInterval);

            if (currencyEconomy.CheckAreaPurchase(maintenanceCost))
            {
                StartMaintenance();

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

        foreach (GameObject obj in affectedObjects)
        {
            obj.SetActive(false);
        }

        Debug.Log("Area maintenance started");
    }

    private void EndMaintenance()
    {
        isUnderMaintenance = false;

        foreach (GameObject obj in affectedObjects)
        {
            obj.SetActive(true);
        }

        Debug.Log("Area maintenance completed");
    }

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
}
