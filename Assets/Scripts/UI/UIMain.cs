using UnityEngine;
using UnityEngine.UI;
using System;

public class UIMain : MonoBehaviour, IScreen
{
    public Text Health;
    public Text Currency;
    public Text UpgradeCost;

    public void Initialize(int health, int currency, int upgradeCost, PlayerState state)
    {
        Health.text = health.ToString();
        Currency.text = currency.ToString();
        UpgradeCost.text = upgradeCost.ToString();
        state.OnHit += HealthChange;
        state.OnCurrencyChanged += CurrencyChange;
    }

    public void Close() =>
        Destroy(gameObject);

    private void HealthChange(int ammount) =>
        Health.text = ammount.ToString();

    private void CurrencyChange(int ammount) =>
        Currency.text = ammount.ToString();
}