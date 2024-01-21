using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DateHistoryItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dateText;

    private string savedDate;

    private void Start()
    {
        SpendingMenu.instance.OnBalanceUpdate += SpendingMenu_OnBalanceUpdate;
        ProfitMenu.instance.OnBalanceUpdate += ProfitMenu_OnBalanceUpdate;
    }

    public void SetDateText(string date)
    {
        savedDate = date;

        dateText.text = date;

        float diff = GetDifference(savedDate);
        if (diff > 0)
            dateText.text = savedDate + "    " + TextColors.ApplyColorToText(TextColors.DefaultColorsEnum.Green, "+" + diff.ToString().Replace(",", "."));
        else if (diff < 0)
            dateText.text = savedDate + "    " + TextColors.ApplyColorToText(TextColors.DefaultColorsEnum.Red, diff.ToString().Replace(",", "."));
        else
            dateText.text = savedDate + "    0";
    }

    private float GetDifference(string date)
    {
        float difference = 0f;

        List<TransactionData> list = DataManager.Instance.GetHistory();
        if (list == null) 
            return difference;

        foreach (TransactionData item in list)
        {
            if (item.date == date)
            {
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

                difference += float.Parse(trimmedString.Replace(".", ","));
            }
        }

        return difference;
    }

    private void SpendingMenu_OnBalanceUpdate(object sender, EventArgs e)
    {
        float diff = GetDifference(savedDate);
        if (diff > 0)
            dateText.text = savedDate + "    " + TextColors.ApplyColorToText(TextColors.DefaultColorsEnum.Green, "+" + diff.ToString().Replace(",", "."));
        else if (diff < 0)
            dateText.text = savedDate + "    " + TextColors.ApplyColorToText(TextColors.DefaultColorsEnum.Red, diff.ToString().Replace(",", "."));
        else
            dateText.text = savedDate + "    0";
    }

    private void ProfitMenu_OnBalanceUpdate(object sender, EventArgs e)
    {
        float diff = GetDifference(savedDate);
        if (diff > 0)
            dateText.text = savedDate + "    " + TextColors.ApplyColorToText(TextColors.DefaultColorsEnum.Green, "+" + diff.ToString().Replace(",", "."));
        else if (diff < 0)
            dateText.text = savedDate + "    " + TextColors.ApplyColorToText(TextColors.DefaultColorsEnum.Red, diff.ToString().Replace(",", "."));
        else
            dateText.text = savedDate + "    0";
    }

    private void OnDestroy()
    {
        SpendingMenu.instance.OnBalanceUpdate -= SpendingMenu_OnBalanceUpdate;
        ProfitMenu.instance.OnBalanceUpdate -= ProfitMenu_OnBalanceUpdate;
    }
}