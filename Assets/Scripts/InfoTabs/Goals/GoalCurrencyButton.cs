using UnityEngine;

public class GoalCurrencyButton : MonoBehaviour
{
    public string currency;
    public int currencyId;

    public void OnCurrencySet()
    {
        NewGoalMenu.instance.SetCurrency(currency, currencyId);
    }
}