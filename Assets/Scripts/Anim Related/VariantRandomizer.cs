using UnityEngine;

public class VariantRandomizer : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    private GameObject currPrefab;
    
    void Start()
    {
        if (prefabs.Length > 0)
        {
            AssignRandomPrefab();
        }
    }

    void AssignRandomPrefab()
    {
        int randomIndex = Random.Range(0, prefabs.Length);
        GameObject selectedPrefab = prefabs[randomIndex];

        currPrefab = Instantiate(selectedPrefab, transform);

        currPrefab.transform.localPosition = Vector3.zero;
        currPrefab.transform.localRotation = Quaternion.identity;   
    }
}
