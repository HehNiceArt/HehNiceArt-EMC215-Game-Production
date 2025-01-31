using UnityEngine;

public class Interactions : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("test");
        Destroy(gameObject);

    }
}
