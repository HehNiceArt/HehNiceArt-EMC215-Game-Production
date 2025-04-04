using UnityEngine;

public class rotattest : MonoBehaviour
{
    public Transform child;
    private Camera mc;

    private void Start()
    {
        mc = Camera.main;

        if (transform.childCount > 0)
        {
            child = transform.GetChild(0);
        }
    }

    private void Update()
    {
        if (child == null)
        {
            if (transform.childCount > 0)
            {
                child = transform.GetChild(0);
            }
        }

        if ( child != null)
        {
            handleOrientation();
        }
    }

    void handleOrientation()
    {
        Vector3 directionToCam = mc.transform.position - child.position;
        directionToCam.y = 0;

        //child.forward = directionToCam.normalized;
        Quaternion targetRotation = Quaternion.LookRotation(-directionToCam);
        child.rotation = targetRotation * Quaternion.Euler(0, 0, 0);
    }

    public void AssignChild(Transform newChild)
    {
        child = newChild;
    }
}
