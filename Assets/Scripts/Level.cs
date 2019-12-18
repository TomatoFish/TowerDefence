using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

public class Level : MonoBehaviour
{
    public Transform CameraPoint;
    public Transform SpawnPoint;
    public Transform TargetPoint;
    public Transform[] TowerPoints;

    private PlayerState state;
    private LevelConfig config;
    private Target target;
    private Transform enemiesContainer;
    private Transform towersContainer;

    public async void Initialize(LevelConfig config)
    {
        this.config = config;
        state = new PlayerState(config.StartHealth);
        state.OnDied += GameOverHandle;
        App.Instance.UI.ShowMainScreen(
            state.Health,
            state.Currency,
            App.Instance.Config.Tower.LevelUpCost,
            state);

        var enemy = await App.Instance.LoadResource(App.Instance.Config.Enemy.Id) as GameObject;
        App.Instance.Pool.Initialize(config.MaxEnemyCount, enemy);

        towersContainer = new GameObject("TowersContainer").transform;
        towersContainer.parent = transform;

        enemiesContainer = new GameObject("EnemyContainer").transform;
        enemiesContainer.parent = transform;

        await SetupMap();
        StartCoroutine(SpawnWaves());
    }

    public bool SpendCurrency(int ammount)
    {
        if (state.Currency >= ammount)
        {
            state.Currency -= ammount;
            return true;
        }
        return false;
    }
    
    private void GameOverHandle()
    {
        target.OnTargetHit -= HitHandler;
        state.OnDied -= GameOverHandle;
        state.Dispose();
        StopAllCoroutines();
        Destroy(towersContainer.gameObject);
        Destroy(enemiesContainer.gameObject);
        App.Instance.UI.ShowGameOverScreen(state.Score, App.Instance.StartNewGame);
    }

    private void HitHandler(IHittable hittable)
    {
        state.Health -= hittable.GetHitCount;
        App.Instance.Pool.ReturnToPool(hittable.GameObject);
    }

    private void EnemyDiedHandler(int ammount, IHittable hittable)
    {
        state.Currency += ammount;
        state.Score++;
        App.Instance.Pool.ReturnToPool(hittable.GameObject);
    }

    private async Task SetupMap()
    {
        App.Instance.Camera.transform.position = CameraPoint.position;

        var targetObj = await App.Instance.LoadResource(config.TargetId) as GameObject;
        target = Instantiate(targetObj, TargetPoint).GetComponent<Target>();
        target.OnTargetHit += HitHandler;

        var towerConfig = App.Instance.Config.Tower;
        var towerObj = await App.Instance.LoadResource(towerConfig.Id) as GameObject;
        foreach (var point in TowerPoints)
        {
            var tower = Instantiate(towerObj, towersContainer).GetComponent<Tower>();
            tower.transform.position = point.position;
            tower.Initialize(towerConfig, this);
        }
    }

    private IEnumerator SpawnWaves()
    {
        var nextWaveTime = config.WaveStartTime;

        while (true)
        {
            state.Wave++;
            var count = Random.Range(config.MinEnemyCount, config.MaxEnemyCount);
            yield return StartCoroutine(SpawnEnemies(count));

            yield return new WaitForSeconds(nextWaveTime);
        }
    }

    private IEnumerator SpawnEnemies(int targetCount)
    {
        for (var count = 0; count < targetCount; count++)
        {
            var newEnemy = App.Instance.Pool.GetFromPool(enemiesContainer).GetComponent<Enemy>();
            newEnemy.Initialize(App.Instance.Config.Enemy, state.Wave, SpawnPoint.position, TargetPoint);
            newEnemy.OnEnemyDied += EnemyDiedHandler;

            while ((SpawnPoint.position - newEnemy.transform.position).magnitude < newEnemy.Agent.radius * 1.5)
                yield return null;
        }
    }
}
