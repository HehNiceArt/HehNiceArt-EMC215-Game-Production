using UnityEngine;

public enum TableLevels
{
    medicalStudent,
    internDoctor,
    resident,
    fellow,
    attendingPhysician
}

[CreateAssetMenu(fileName = "SO_TableBehavior", menuName = "Hospital Cats/SO_TableBehavior")]
public class SO_TableBehavior : ScriptableObject
{
    private static bool s_tableIsLocked = true;
    public TableLevels tableLevels;
    public Texture2D[] staffProfile;
    public float treatmentCost = 1.5f;
    public float salary = 167;
    public float costToHire = 250;
    public Material notPurchased;

    public bool tableIsLocked
    {
        get => s_tableIsLocked;
        set => s_tableIsLocked = value;
    }

    private void OnEnable()
    {
        // Reset the static state when the ScriptableObject is loaded
        s_tableIsLocked = true;
    }
}
