using UnityEngine;

public class ResetAreaStats : MonoBehaviour
{
    [SerializeField] SO_AreaDetails[] areaDetails;

    public void ResetLocks()
    {
        foreach (SO_AreaDetails area in areaDetails)
        {
            area.isLocked = true;
        }
    }
}
