using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("Header")]
    [SerializeField] private TextMeshProUGUI currentDateText;
    [SerializeField] private TextMeshProUGUI totalBalanceText;

    [Space]
    [SerializeField] private TextMeshProUGUI spendingsText;
    [SerializeField] private TextMeshProUGUI profitText;

    [Space]
    [SerializeField] private GameObject NewCardMenu;

    private float totalBalance;
    private int balanceIdInList = 1;

    private string mainCurrency;

    // Start is called before the first frame update
    private void Start()
    {
        // Attach events (creating of new card and succesful spending)
        Card.OnCardCreate += Card_OnCardCreate;
        Card.OnCardDelete += Card_OnCardDelete;
        SpendingMenu.instance.OnBalanceUpdate += SpendingMenu_OnBalanceUpdate;
        ProfitMenu.instance.OnBalanceUpdate += ProfitMenu_OnBalanceUpdate;
        EditCardMenu.instance.OnBalanceUpdate += EditCardMenu_OnBalanceUpdate;

        currentDateText.text = char.ToUpper(DateTime.Now.ToString("MMMM, yyyy")[0]) + DateTime.Now.ToString("MMMM, yyyy").Substring(1);

        mainCurrency = PlayerPrefs.GetString("MainCurrency", "$");

        SetTotalCardBalance();
    }

    private void Update()
    {
        SetTotalCardBalance();
        SetMonthSpendings();
        SetMonthProfit();
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

    private void SetTotalCardBalance()
    {
        totalBalance = 0;

        Card.LoadCardList();

        foreach (string[] card in Card.CardList)
        {
            totalBalance += CurrencyConverter.instance.GetConvertedValue(float.Parse(card[balanceIdInList].Replace(".", ",")), card[3]);
        }

        double doubleVal = Convert.ToDouble(totalBalance);
        doubleVal = Math.Round(doubleVal, 2);
        string cardBalance = doubleVal.ToString();

        totalBalanceText.text = TextColors.ApplyColorToText(TextColors.DefaultColorsEnum.Green, mainCurrency) + TextColors.ApplyColorToText(TextColors.DefaultColorsEnum.Black, cardBalance);
    }

    private void SetMonthSpendings()
    {
        List<TransactionData> transactionsList = DataManager.Instance.GetHistory();

        float summ = 0f;
        if (transactionsList == null)
        {
            spendingsText.text = summ.ToString();
        }
        else
        {
            foreach (TransactionData transactionData in transactionsList)
            {
                if (transactionData.transactionSum.Contains("-"))
                {
                    int indexMinus = transactionData.transactionSum.IndexOf('-');

                    // Find the earliest occurrence of either '+' or '-'
                    int cutIndex = -1;
                    if (indexMinus != -1)
                    {
                        cutIndex = indexMinus;
                    }
                    string trimmedString = cutIndex != -1 ? transactionData.transactionSum.Substring(cutIndex + 1) : transactionData.transactionSum;

                    summ += CurrencyConverter.instance.GetConvertedValue(float.Parse(trimmedString.Replace(".", ",")), transactionData.currencyCode);
                }
            }

            double doubleVal = Convert.ToDouble(summ);
            doubleVal = Math.Round(doubleVal, 2);

            spendingsText.text = mainCurrency + doubleVal.ToString().Replace(",", ".");
        }
    }

    private void SetMonthProfit()
    {
        List<TransactionData> transactionsList = DataManager.Instance.GetHistory();

        float summ = 0f;
        if (transactionsList == null)
        {
            profitText.text = summ.ToString();
        }
        else
        {
            foreach (TransactionData transactionData in transactionsList)
            {
                if (transactionData.transactionSum.Contains("+"))
                {
                    int indexPlus = transactionData.transactionSum.IndexOf('+');

                    // Find the earliest occurrence of either '+' or '-'
                    int cutIndex = -1;
                    if (indexPlus != -1)
                    {
                        cutIndex = indexPlus;
                    }
                    string trimmedString = cutIndex != -1 ? transactionData.transactionSum.Substring(cutIndex + 1) : transactionData.transactionSum;

                    summ += CurrencyConverter.instance.GetConvertedValue(float.Parse(trimmedString.Replace(".", ",")), transactionData.currencyCode);
                }
            }

            double doubleVal = Convert.ToDouble(summ);
            doubleVal = Math.Round(doubleVal, 2);

            profitText.text = mainCurrency + doubleVal.ToString().Replace(",", ".");
        }
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