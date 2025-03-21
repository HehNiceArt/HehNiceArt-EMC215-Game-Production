using UnityEngine;

public enum TableLevels
{
    medicalStudent,
    internDoctor,
    resident,
    fellow,
    attendingPhysician
}
public class SO_TableBehavior
{
    public TableLevels tableLevels;
    public GameObject staffProfile;
    public float treatmentCost = 1.5f;
    public float salary = 167;
    public float costToHire = 250;
    public float xpGain = 78;
    public bool tableIsLocked = true;

    public void UpdateValuesBasedOnLevel()
    {
        switch (tableLevels)
        {
            case TableLevels.medicalStudent:
                treatmentCost = 1.5f;
                salary = 167f;
                costToHire = 250f;
                xpGain = 158f;
                break;
            case TableLevels.internDoctor:
                treatmentCost = 2f;
                salary = 467f;
                costToHire = 700f;
                xpGain = 264f;
                break;
            case TableLevels.resident:
                treatmentCost = 2.75f;
                salary = 667f;
                costToHire = 1000f;
                xpGain = 316f;
                break;
            case TableLevels.fellow:
                treatmentCost = 3.75f;
                salary = 2333f;
                costToHire = 3500f;
                xpGain = 592f;
                break;
            case TableLevels.attendingPhysician:
                treatmentCost = 4.75f;
                salary = 4000f;
                costToHire = 6000f;
                xpGain = 775f;
                break;
        }
    }
}
