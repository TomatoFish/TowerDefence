using UnityEngine;

public class Tower : MonoBehaviour
{
    public Weapon Weapon;

    private TowerConfig config;
    private int level;
    private Level currentLevel;

    public void Initialize(TowerConfig config, Level currentLevel)
    {
        this.config = config;
        this.currentLevel = currentLevel;
        level = 0;
        Weapon.Initialize(config);
        Weapon.StatsUpdate(level);
    }

    private void OnMouseDown()
    {
        if (currentLevel.SpendCurrency(config.LevelUpCost))
            Weapon.StatsUpdate(++level);
    }
}
