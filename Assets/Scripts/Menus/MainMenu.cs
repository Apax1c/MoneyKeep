using System;
using System.Collections.Generic;
using System.Globalization;
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
        totalBalance = 0f;

        Card.LoadCardList();

        foreach (string[] card in Card.CardList)
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            var decimalSeparator = culture.NumberFormat.NumberDecimalSeparator;

            // Підготовка рядка для конвертації
            string preparedBalance = decimalSeparator == "," ? card[balanceIdInList].Replace(".", ",") : card[balanceIdInList].Replace(",", ".");

            if (float.TryParse(preparedBalance, NumberStyles.Any, culture, out float cardBalance))
            {
                totalBalance += CurrencyConverter.instance.GetConvertedValue(cardBalance, card[3]);
            }
        }

        // Округлення до двох знаків після коми
        totalBalance = (float)Math.Round(totalBalance, 2);

        // Форматування рядка балансу
        string cardBalanceText = totalBalance.ToString("F2", CultureInfo.CurrentCulture);

        totalBalanceText.text = TextColors.ApplyColorToText(TextColors.DefaultColorsEnum.Green, mainCurrency) +
                                TextColors.ApplyColorToText(TextColors.DefaultColorsEnum.Black, cardBalanceText);
    }



    private void SetMonthSpendings()
    {
        List<TransactionData> transactionsList = DataManager.Instance.GetHistory();

        float summ = 0f;
        if (transactionsList == null)
        {
            spendingsText.text = mainCurrency + "0.00";
            return;
        }

        foreach (TransactionData transactionData in transactionsList)
        {
            if (transactionData.transactionSum.Contains("-"))
            {
                int indexMinus = transactionData.transactionSum.IndexOf('-');

                string trimmedString = indexMinus != -1 ? transactionData.transactionSum.Substring(indexMinus + 1) : transactionData.transactionSum;
                if (float.TryParse(trimmedString.Replace('.', ','), NumberStyles.Any, CultureInfo.InvariantCulture, out float parsedValue))
                {
                    summ += CurrencyConverter.instance.GetConvertedValue(parsedValue, transactionData.currencyCode);
                }
            }
        }

        double doubleVal = Math.Round(Convert.ToDouble(summ), 2);

        spendingsText.text = mainCurrency + doubleVal.ToString("F2").Replace(",", ".");
    }


    private void SetMonthProfit()
    {
        List<TransactionData> transactionsList = DataManager.Instance.GetHistory();

        float summ = 0f;
        if (transactionsList == null)
        {
            profitText.text = "0.00";
            return;
        }

        foreach (TransactionData transactionData in transactionsList)
        {
            if (transactionData.transactionSum.Contains("+"))
            {
                int indexPlus = transactionData.transactionSum.IndexOf('+');

                string trimmedString = indexPlus != -1 ? transactionData.transactionSum.Substring(indexPlus + 1) : transactionData.transactionSum;
                if (float.TryParse(trimmedString.Replace('.', ','), NumberStyles.Any, CultureInfo.InvariantCulture, out float parsedValue))
                {
                    summ += CurrencyConverter.instance.GetConvertedValue(parsedValue, transactionData.currencyCode);
                }
            }
        }

        double doubleVal = Math.Round(Convert.ToDouble(summ), 2);
        profitText.text = mainCurrency + doubleVal.ToString("F2").Replace(",", ".");
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