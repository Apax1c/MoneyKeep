using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class DateHistoryItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dateText;

    private string savedDate;

    private string mainCurrency;

    private void Awake()
    {
        mainCurrency = PlayerPrefs.GetString("MainCurrency", "$");
    }

    private void Start()
    {
        SpendingMenu.instance.OnBalanceUpdate += SpendingMenu_OnBalanceUpdate;
        ProfitMenu.instance.OnBalanceUpdate += ProfitMenu_OnBalanceUpdate;
    }

    public void SetDateText(string date)
    {
        savedDate = date;

        dateText.text = date;

        double diff = GetDifference(savedDate);
        if (diff > 0)
            dateText.text = savedDate + "    " + TextColors.ApplyColorToText(TextColors.DefaultColorsEnum.Green, mainCurrency + diff.ToString().Replace(",", "."));
        else if (diff < 0)
            dateText.text = savedDate + "    " + TextColors.ApplyColorToText(TextColors.DefaultColorsEnum.Red, mainCurrency + diff.ToString().Replace(",", ".").Substring(1));
        else
            dateText.text = savedDate + "    0" + mainCurrency;
    }

    public void SetDateText(string date, string categoryName)
    {
        savedDate = date;

        dateText.text = date;

        double diff = GetDifference(savedDate, categoryName);
        if (diff > 0)
            dateText.text = savedDate + "    " + TextColors.ApplyColorToText(TextColors.DefaultColorsEnum.Green, mainCurrency + diff.ToString().Replace(",", "."));
        else if (diff < 0)
            dateText.text = savedDate + "    " + TextColors.ApplyColorToText(TextColors.DefaultColorsEnum.Red, mainCurrency + diff.ToString().Replace(",", ".").Substring(1));
        else
            dateText.text = savedDate + "    0" + mainCurrency;
    }

    private double GetDifference(string date, string categoryName)
    {
        float difference = 0f;

        List<TransactionData> list = DataManager.Instance.GetHistory();
        if (list == null)
            return 0;

        foreach (TransactionData item in list)
        {
            if (item.transactionName != categoryName || item.date != date)
                continue;

            int indexPlus = item.transactionSum.IndexOf('+');
            int indexMinus = item.transactionSum.IndexOf('-');

            // Find the earliest occurrence of either '+' or '-'
            int cutIndex = -1;
            if (indexPlus != -1 && indexMinus != -1)
            {
                cutIndex = Math.Min(indexPlus, indexMinus);
            }
            else if (indexPlus != -1)
            {
                cutIndex = indexPlus;
            }
            else if (indexMinus != -1)
            {
                cutIndex = indexMinus;
            }

            string trimmedString = cutIndex != -1 ? item.transactionSum.Substring(cutIndex) : item.transactionSum;

            // Use InvariantCulture to ensure the decimal point is correctly interpreted
            if (float.TryParse(trimmedString.Replace('.', ','), NumberStyles.Any, CultureInfo.InvariantCulture, out float parsedValue))
            {
                difference += CurrencyConverter.instance.GetConvertedValue(parsedValue, item.currencyCode);
            }
        }

        // Convert to double for the return type
        return Math.Round(Convert.ToDouble(difference), 2);
    }

    private double GetDifference(string date)
    {
        float difference = 0f;

        List<TransactionData> list = DataManager.Instance.GetHistory();
        if (list == null)
            return 0;

        foreach (TransactionData item in list)
        {
            if (item.date == date)
            {
                int indexPlus = item.transactionSum.IndexOf('+');
                int indexMinus = item.transactionSum.IndexOf('-');

                // Find the earliest occurrence of either '+' or '-'
                int cutIndex = indexPlus != -1 ? indexPlus : indexMinus;

                string trimmedString = cutIndex != -1 ? item.transactionSum.Substring(cutIndex) : item.transactionSum;

                if (float.TryParse(trimmedString.Replace('.', ','), NumberStyles.Any, CultureInfo.InvariantCulture, out float parsedValue))
                {
                    difference += CurrencyConverter.instance.GetConvertedValue(parsedValue, item.currencyCode);
                }
            }
        }

        return Math.Round(difference, 2);
    }


    private void SpendingMenu_OnBalanceUpdate(object sender, EventArgs e)
    {
        double diff = GetDifference(savedDate);
        if (diff > 0)
            dateText.text = savedDate + "    " + TextColors.ApplyColorToText(TextColors.DefaultColorsEnum.Green, mainCurrency + diff.ToString().Replace(",", "."));
        else if (diff < 0)
            dateText.text = savedDate + "    " + TextColors.ApplyColorToText(TextColors.DefaultColorsEnum.Red, mainCurrency + diff.ToString().Replace(",", ".").Substring(1));
        else
            dateText.text = savedDate + "    0" + mainCurrency;
    }

    private void ProfitMenu_OnBalanceUpdate(object sender, EventArgs e)
    {
        double diff = GetDifference(savedDate);
        if (diff > 0)
            dateText.text = savedDate + "    " + TextColors.ApplyColorToText(TextColors.DefaultColorsEnum.Green, mainCurrency + diff.ToString().Replace(",", "."));
        else if (diff < 0)
            dateText.text = savedDate + "    " + TextColors.ApplyColorToText(TextColors.DefaultColorsEnum.Red, mainCurrency + diff.ToString().Replace(",", ".").Substring(1));
        else
            dateText.text = savedDate + "    0" + mainCurrency;
    }

    private void OnDestroy()
    {
        SpendingMenu.instance.OnBalanceUpdate -= SpendingMenu_OnBalanceUpdate;
        ProfitMenu.instance.OnBalanceUpdate -= ProfitMenu_OnBalanceUpdate;
    }
}