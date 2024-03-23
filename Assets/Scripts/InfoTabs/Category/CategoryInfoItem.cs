using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;

public class CategoryInfoItem : MonoBehaviour
{
    [SerializeField] private SVGImage backgroundImage;
    [SerializeField] private SVGImage iconImage;

    [SerializeField] private TextMeshProUGUI differenceSumText;
    [SerializeField] private TextMeshProUGUI categoryNameText;

    private string mainCurrency;
    private int categoryId;

    private void Start()
    {
        mainCurrency = PlayerPrefs.GetString("MainCurrency", "$");
    }

    private void Update()
    {
        differenceSumText.text = TextColors.ApplyColorToText(iconImage.color, GetCategorySumm(categoryNameText.text));
    }

    public void OnButtonClicked()
    {
        CategoryInfoMenu.instance.OpenMenu(categoryId);
    }

    public void SetCategory(CategoryData data)
    {
        categoryId = data.id;

        categoryNameText.text = Localisation.GetString(data.categoryName, this);
        differenceSumText.text = TextColors.ApplyColorToText(data.categoryIconColor, GetCategorySumm(data.categoryName));

        backgroundImage.color = data.categoryColor;
        iconImage.sprite = data.categoryIcon;
        iconImage.color = data.categoryIconColor;
    }

    private string GetCategorySumm(string categoryName)
    {
        List<TransactionData> transactionsList = DataManager.Instance.GetHistory();

        float summ = 0f;
        if (transactionsList == null)
        {
            return mainCurrency + "0.00";
        }

        foreach (TransactionData transactionData in transactionsList)
        {
            if (transactionData.transactionName.Contains(categoryName))
            {
                int indexMinus = transactionData.transactionSum.IndexOf('-');

                string trimmedString = indexMinus != -1 ? transactionData.transactionSum.Substring(indexMinus + 1) : transactionData.transactionSum;

                if (float.TryParse(trimmedString.Replace('.', ','), NumberStyles.Any, CultureInfo.InvariantCulture, out float parsedValue))
                {
                    summ += CurrencyConverter.instance.GetConvertedValue(parsedValue, transactionData.currencyCode);
                }
            }
        }

        float roundedSum = (float)Math.Round(summ, 2);
        return mainCurrency + roundedSum.ToString("F2").Replace(",", ".");
    }

}
