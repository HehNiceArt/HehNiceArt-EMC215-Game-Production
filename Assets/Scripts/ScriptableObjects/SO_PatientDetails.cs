using UnityEngine;

public enum PatientSex
{
    male,
    female
}
[CreateAssetMenu(fileName = "SO_PatientDetails", menuName = "Hospital Cats/SO_PatientDetails")]
public class SO_PatientDetails : ScriptableObject
{
    public Texture2D[] patientProfile;
    public string[] patientName;
    public float treatmentTime;
    public float coinDrops;
}
