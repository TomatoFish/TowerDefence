using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Configs/EnemyConfig", fileName = "EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    public string Id;
    public float Health;
    public float Speed;
    public int Damage;
    public int Reward;

    public float HealthLevelMultiplier;
    public float SpeedLevelMultiplier;
}
