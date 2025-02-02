using UnityEngine;

[CreateAssetMenu(fileName = "SO_MedicalStaff", menuName = "Hospital Cats/SO_MedicalStaff")]
public class SO_MedicalStaff : ScriptableObject
{
    public Texture2D staffProfile;
    public string staffName;
    public float treatmentTime;
    public float cost;
}
