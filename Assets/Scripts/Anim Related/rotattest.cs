using UnityEngine;

public class rotattest : MonoBehaviour
{
    public Transform child;
    public Transform target;

    private void Start()
    {
        if (transform.childCount > 0)
        {
            child = transform.GetChild(0);
        }

        target = GameObject.Find("Screen")?.transform;
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

        if ( child != null && target != null)
        {
            handleOrientation();
        }
    }

    void handleOrientation()
    {
        Vector3 directionToTarget = target.transform.position - child.position;
        directionToTarget.y = 0;

        //child.forward = directionToCam.normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget.normalized);
        child.rotation = targetRotation * Quaternion.Euler(0, 0, 0);
    }

    /*public void AssignChild(Transform newChild)
    {
        child = newChild;
    }*/
}
