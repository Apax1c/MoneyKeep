using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentDateText;
    [SerializeField] private TextMeshProUGUI totalBalanceText;

    [SerializeField] private GameObject NewCardMenu;

    private float totalBalance;
    private int balanceIdInList = 1;

    // Start is called before the first frame update
    private void Start()
    {
        // Attach events (creating of new card and succesful spending)
        Card.OnCardCreate += Card_OnCardCreate;
        Card.OnCardDelete += Card_OnCardDelete;
        SpendingMenu.instance.OnBalanceUpdate += SpendingMenu_OnBalanceUpdate;
        ProfitMenu.instance.OnBalanceUpdate += ProfitMenu_OnBalanceUpdate;
        EditCardMenu.instance.OnBalanceUpdate += EditCardMenu_OnBalanceUpdate;

        currentDateText.text = (char.ToUpper(DateTime.Now.ToString("MMMM, yyyy")[0]) + DateTime.Now.ToString("MMMM, yyyy").Substring(1));
        
        SetTotalCardBalance();
    }

    private void Card_OnCardCreate(object sender, EventArgs e)
    {
        SetTotalCardBalance();
    }

    private void Card_OnCardDelete(object sender, EventArgs e)
    {
        SetTotalCardBalance();
    }

    private void SpendingMenu_OnBalanceUpdate(object sender, EventArgs e)
    {
        SetTotalCardBalance();
    }

    private void ProfitMenu_OnBalanceUpdate(object sender, EventArgs e)
    {
        SetTotalCardBalance();
    }

    private void EditCardMenu_OnBalanceUpdate(object sender, EventArgs e)
    {
        SetTotalCardBalance();
    }

    private void Update()
    {
        SetTotalCardBalance();
    }

    private void SetTotalCardBalance()
    {
        totalBalance = 0;

        Card.LoadCardList();

        foreach (string[] card in Card.CardList)
        {
            totalBalance += GetConvertedValue(float.Parse(card[balanceIdInList].Replace(".", ",")), card[3]);
        }

        double doubleVal = Convert.ToDouble(totalBalance);
        doubleVal = Math.Round(doubleVal, 2);
        string cardBalance = doubleVal.ToString();

        totalBalanceText.text = TextColors.ApplyColorToText(TextColors.DefaultColorsEnum.Green, "$") + TextColors.ApplyColorToText(TextColors.DefaultColorsEnum.Black, cardBalance);
    }

    private float GetConvertedValue(float value, string currencyCode)
    {
        var convertedValue = currencyCode switch
        {
            "USD" => CurrencyConverter.instance.ConvertUSD(value),
            "EUR" => CurrencyConverter.instance.ConvertEUR(value),
            "UAH" => CurrencyConverter.instance.ConvertUAH(value),
            "JPY" => CurrencyConverter.instance.ConvertJPY(value),
            "AUD" => CurrencyConverter.instance.ConvertAUD(value),
            "CAD" => CurrencyConverter.instance.ConvertCAD(value),
            "GBP" => CurrencyConverter.instance.ConvertGBP(value),
            "CHF" => CurrencyConverter.instance.ConvertCHF(value),
            "NOK" => CurrencyConverter.instance.ConvertNOK(value),
            "CNY" => CurrencyConverter.instance.ConvertCNY(value),
            "SEK" => CurrencyConverter.instance.ConvertSEK(value),
            _ => CurrencyConverter.instance.ConvertUSD(value),
        };
        return convertedValue;
    }

    private void OnDisable()
    {
        Card.OnCardCreate -= Card_OnCardCreate;
        Card.OnCardDelete -= Card_OnCardDelete;
        SpendingMenu.instance.OnBalanceUpdate -= SpendingMenu_OnBalanceUpdate;
        ProfitMenu.instance.OnBalanceUpdate -= ProfitMenu_OnBalanceUpdate;
        EditCardMenu.instance.OnBalanceUpdate -= EditCardMenu_OnBalanceUpdate;
    }
}