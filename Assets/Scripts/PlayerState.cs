using System;
using UnityEngine;

public class PlayerState
{
    public Action OnDied;
    public Action<int> OnHit;
    public Action<int> OnCurrencyChanged;

    public int Score;
    public int Wave;

    private int currency;
    public int Currency
    {
        get => currency;
        set
        {
            currency = value;
            OnCurrencyChanged?.Invoke(currency);
        }
    }

    private int health;
    public int Health
    {
        get => health;
        set
        {
            if (value < health)
                health = Mathf.Max(value, 0);
            else
                health = value;

            OnHit?.Invoke(health);
            if (health == 0) OnDied?.Invoke();
        }
    }

    public PlayerState(int health)
    {
        currency = 0;
        Score = 0;
        Wave = 0;
        this.health = health;
    }

    public void Dispose()
    {
        OnDied = null;
        OnHit = null;
        OnCurrencyChanged = null;
    }
}
