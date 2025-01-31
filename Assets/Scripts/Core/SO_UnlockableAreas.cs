using UnityEngine;

[CreateAssetMenu(fileName = "SO_UnlockableAreas", menuName = "Hospital Cats/SO_UnlockableAreas")]
public class SO_UnlockableAreas : ScriptableObject
{

    [Header("First Area")]
    public bool isFirstArea = false;
    public GameObject firstArea;

    [Header("Second Area")]
    public bool isSecondArea = false;
    public GameObject secondArea;

    [Header("Third Area")]
    public bool isThirdArea = false;
    public GameObject thirdArea;
    [Header("Fourth Area")]
    public bool isFourthArea = false;
    public GameObject fourthArea;
    [Header("Fifth Area")]
    public bool isFifthArea = false;
    public GameObject fifthArea;
}
