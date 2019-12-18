using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class App : MonoBehaviour
{
    public Pool Pool;
    public UIManager UI;
    public Camera Camera;

    private Level currentLevel;

    private static App instance;
    public static App Instance => instance ? instance : instance = FindObjectOfType<App>();

    public GameConfig Config { get; private set; }

    private const string configPath = "Definitions/GameConfig";

    private async void Start()
    {
        Config = await LoadResource(configPath) as GameConfig;
        StartNewGame();
    }

    public async void StartNewGame()
    {
        var levelConfig = Config.Level;
        currentLevel = await LoadLevel(levelConfig);
        currentLevel.Initialize(levelConfig);
    }

    public async Task<Object> LoadResource(string name)
    {
        var request = Resources.LoadAsync(name);
        while (!request.isDone) await Task.Yield();
        return request.asset;
    }

    private async Task<Level> LoadLevel(LevelConfig config)
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
            await UnloadScene();
        }

        await LoadScene(config.Id);
        var newLevel = await LoadResource($"Levels/{config.Id}") as GameObject;
        var level = Instantiate(newLevel, transform).GetComponent<Level>();
        return level;
    }

    private async Task LoadScene(string id)
    {
        var ao = SceneManager.LoadSceneAsync(id, LoadSceneMode.Additive);
        while (!ao.isDone) await Task.Yield();

        var scene = SceneManager.GetSceneByName(id);
        SceneManager.SetActiveScene(scene);
    }

    private async Task UnloadScene()
    {
        var scene = SceneManager.GetActiveScene();
        if (scene.isLoaded)
        {
            var ao = SceneManager.UnloadSceneAsync(scene);
            while (!ao.isDone) await Task.Yield();
        }
    }
}
