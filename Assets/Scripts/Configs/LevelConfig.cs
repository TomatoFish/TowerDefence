using UnityEngine;

[CreateAssetMenu(menuName = "Configs/LevelConfig", fileName = "LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public string Id;
    public int StartHealth;
    public int MinEnemyCount;
    public int MaxEnemyCount;
    public float WaveStartTime;
    public string TargetId;
}
