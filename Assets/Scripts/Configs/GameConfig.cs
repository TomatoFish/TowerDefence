using UnityEngine;

[CreateAssetMenu(menuName = "Configs/GameConfig", fileName = "GameConfig")]
public class GameConfig : ScriptableObject
{
    public LevelConfig Level;
    public TowerConfig Tower;
    public EnemyConfig Enemy;
}
