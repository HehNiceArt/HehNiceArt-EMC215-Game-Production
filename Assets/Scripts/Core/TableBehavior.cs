using UnityEngine;

public enum TableLevels
{
    Medical_Student,
    Intern_Doctor,
    Resident,
    Fellow,
    Attending_Physician
}
public class TableBehavior
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
            case TableLevels.Medical_Student:
                treatmentCost = 1.5f;
                salary = 167f;
                costToHire = 250f;
                xpGain = 158f;
                break;
            case TableLevels.Intern_Doctor:
                treatmentCost = 2f;
                salary = 467f;
                costToHire = 700f;
                xpGain = 264f;
                break;
            case TableLevels.Resident:
                treatmentCost = 2.75f;
                salary = 667f;
                costToHire = 1000f;
                xpGain = 316f;
                break;
            case TableLevels.Fellow:
                treatmentCost = 3.75f;
                salary = 2333f;
                costToHire = 3500f;
                xpGain = 592f;
                break;
            case TableLevels.Attending_Physician:
                treatmentCost = 4.75f;
                salary = 4000f;
                costToHire = 6000f;
                xpGain = 775f;
                break;
        }
    }
}
