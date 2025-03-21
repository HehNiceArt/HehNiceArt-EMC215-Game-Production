using UnityEngine;

[CreateAssetMenu(fileName = "SO_PlayerDetails", menuName = "Hospital Cats/SO_PlayerDetails")]
public class SO_PlayerDetails : ScriptableObject
{
    public float coins = 1500f;
    public float reputation = 0;
    public float playerLevel = 1;

    public const float MIN_REPUTATION = -100f;
    public const float REPUTATION_TIER_1 = 50f;
    public const float REPUTATION_TIER_2 = 150f;
    public const float REPUTATION_TIER_3 = 300f;
    public const float REPUTATION_TIER_4 = 500f;
}
