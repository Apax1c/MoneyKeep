using System;
using System.Collections.Generic;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using static UnityEditor.Progress;

public class CategoryInfoItem : MonoBehaviour
{
    [SerializeField] private SVGImage backgroundImage;
    [SerializeField] private SVGImage iconImage;

    [SerializeField] private TextMeshProUGUI differenceSumText;
    [SerializeField] private TextMeshProUGUI categoryNameText;

    private int categoryId;

    private void Update()
    {
        differenceSumText.text = TextColors.ApplyColorToText(iconImage.color, GetCategorySumm(categoryNameText.text));
    }

    public void SetCategory(CategoryData data)
    {
        categoryId = data.id;

        categoryNameText.text = data.categoryName;
        differenceSumText.text = TextColors.ApplyColorToText(data.categoryIconColor, GetCategorySumm(data.categoryName));

        backgroundImage.color = data.categoryColor;
        iconImage.sprite = data.categoryIcon;
        iconImage.color = data.categoryIconColor;
    }

    private string GetCategorySumm(string categoryName)
    {
        List<TransactionData> transactionsList = DataManager.Instance.GetHistory();
        
        float summ = 0f;
        if(transactionsList == null)
        {
            return summ.ToString();
        }

        foreach (TransactionData transactionData in transactionsList)
        {
            if (transactionData.transactionName.Contains(categoryName))
            {
                int indexMinus = transactionData.transactionSum.IndexOf('-');

                // Find the earliest occurrence of either '+' or '-'
                int cutIndex = -1;
                if (indexMinus != -1)
                {
                    cutIndex = indexMinus;
                }
                string trimmedString = cutIndex != -1 ? transactionData.transactionSum.Substring(cutIndex + 1) : transactionData.transactionSum;

                summ += float.Parse(trimmedString.Replace(".", ","));
            }
        }

        return summ.ToString().Replace(",", ".");
    }
}
