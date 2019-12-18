using UnityEngine;

[CreateAssetMenu(menuName = "Configs/TowerConfig", fileName = "TowerConfig")]
public class TowerConfig : ScriptableObject
{
    public string Id;
    public int LevelUpCost;
    public float Damage;
    public float FireRate;
    public float DamageLevelMultiplier;
    public float FireRateMultiplier;
    public float FireRateMin;
}
