using UnityEngine;

public class UILerp : MonoBehaviour
{
    [SerializeField] GameObject self;


    void Update()
    {
        float newY = Mathf.Sin(Time.time * 1f);
        self.transform.position = new Vector3(self.transform.position.x, newY, self.transform.position.z) * 1.5f;
    }
}
