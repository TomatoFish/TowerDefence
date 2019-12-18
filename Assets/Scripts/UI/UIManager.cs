using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    private IScreen currentScreen;

    private string uiPath = "UI/";

    public async void ShowGameOverScreen(int score, UnityAction callback)
    {
        var goScreen = await ShowScreen<UIGameOver>(nameof(UIGameOver));
        goScreen.Initialize(score, callback);
    }

    public async void ShowMainScreen(int health, int currency, int upgradeCost, PlayerState state)
    {
        var mainScreen = await ShowScreen<UIMain>(nameof(UIMain));
        mainScreen.Initialize(health, currency, upgradeCost, state);
    }

    public async Task<T> ShowScreen<T>(string name) where T : IScreen
    {
        currentScreen?.Close();
        var screen = await SetScreen<T>(name);
        currentScreen = screen;
        return screen;
    }

    private async Task<T> SetScreen<T>(string name)
    {
        var screenObj = await App.Instance.LoadResource(uiPath + name) as GameObject;
        var newScreen = Instantiate(screenObj, transform).GetComponent<T>();
        return newScreen;
    }
}
