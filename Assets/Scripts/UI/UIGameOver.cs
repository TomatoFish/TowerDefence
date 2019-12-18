using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIGameOver : MonoBehaviour, IScreen
{
    public Text Score;
    public Button Repeat;

    public void Initialize(int score, UnityAction callback)
    {
        Score.text = score.ToString();
        Repeat.onClick.AddListener(callback);
    }

    public void Close()
    {
        Repeat.onClick.RemoveAllListeners();
        Destroy(gameObject);
    }
}
