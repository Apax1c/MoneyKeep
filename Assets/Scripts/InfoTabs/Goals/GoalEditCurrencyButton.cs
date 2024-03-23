using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalEditCurrencyButton : MonoBehaviour
{
    public string currency;
    public int currencyId;

    public void OnCurrencySet()
    {
        EditGoalMenu.instance.SetCurrency(currency, currencyId);
    }
}
