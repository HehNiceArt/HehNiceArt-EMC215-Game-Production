using UnityEngine;

public enum PatientType
{
    common,
    highProfile
}
[CreateAssetMenu(fileName = "SO_PatientDetails", menuName = "Hospital Cats/SO_PatientDetails")]
public class SO_PatientDetails : ScriptableObject
{
    public PatientType patientType;
    public Texture2D[] patientProfile;
    public string[] patientName;
    public float waitingPeriod;
    public float treatmentTime;
    public float coinDrops;
    public float xpDrop;
    public float reputation;
}
