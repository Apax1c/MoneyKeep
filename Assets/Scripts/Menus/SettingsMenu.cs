using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    private Animator settingsMenuAnimator;
    private const string IS_MENU_TOGGLED = "isMenuToggled";
    private bool isMenuToggled = false;

    private void Start()
    {
        settingsMenuAnimator = GetComponent<Animator>();
    }

    public void OpenMenu()
    {
        isMenuToggled = true;
        settingsMenuAnimator.SetBool(IS_MENU_TOGGLED, isMenuToggled);
    }

    public void CloseMenu()
    {
        isMenuToggled = false;
        settingsMenuAnimator.SetBool(IS_MENU_TOGGLED, isMenuToggled);
    }

    public void SetMainCurrency(int id)
    {
        string currency;
        string currencyCode;

        switch (id)
        {
            case 0:
                currencyCode = "USD";
                currency = "$";
                break;
            case 1:
                currencyCode = "EUR";
                currency = "€";
                break;
            case 2:
                currencyCode = "UAH";
                currency = "₴";
                break;
            case 3:
                currencyCode = "JPY";
                currency = "¥";
                break;
            case 4:
                currencyCode = "AUD";
                currency = "$";
                break;
            case 5:
                currencyCode = "CAD";
                currency = "$";
                break;
            case 6:
                currencyCode = "GBP";
                currency = "£";
                break;
            case 7:
                currencyCode = "CHF";
                currency = "₣";
                break;
            case 8:
                currencyCode = "NOK";
                currency = "kr";
                break;
            case 9:
                currencyCode = "CNY";
                currency = "¥";
                break;
            case 10:
                currencyCode = "SEK";
                currency = "kr";
                break;
            default:
                currencyCode = "USD";
                currency = "$";
                break;
        }


        PlayerPrefs.SetString("MainCurrency", currency);
        PlayerPrefs.SetString("MainCurrencyCode", currencyCode);
        PlayerPrefs.Save();

        StartCoroutine(CurrencyConverter.instance.GetExchangeRate());
    }
}
