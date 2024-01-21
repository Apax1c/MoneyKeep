using System;
using System.Collections.Generic;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;

public class CategoryInfoMenu : MonoBehaviour
{
    public static CategoryInfoMenu instance;

    public Animator categoryInfoMenuAnimator;
    public const string IS_MENU_TOGGLED = "isMenuToggled";
    public bool isMenuToggled = false;

    [Space]
    [SerializeField] private TextMeshProUGUI categoryNameText;

    [Header("Markers")]
    [SerializeField] private SVGImage dailyMarker;
    [SerializeField] private SVGImage weeklyMarker;
    [SerializeField] private SVGImage monthlyMarker;

    [Header("Daily Info")]
    [SerializeField] private TextMeshProUGUI dailyText;
    [SerializeField] private TextMeshProUGUI dailyDateText;
    [SerializeField] private TextMeshProUGUI dailySumText;

    [Header("Weekly Info")]
    [SerializeField] private TextMeshProUGUI weeklyText;
    [SerializeField] private TextMeshProUGUI weeklyDateText;
    [SerializeField] private TextMeshProUGUI weeklySumText;

    [Header("Monthly Info")]
    [SerializeField] private TextMeshProUGUI monthlyText;
    [SerializeField] private TextMeshProUGUI monthlyInfoDateText;
    [SerializeField] private TextMeshProUGUI monthlySumText;

    private CategoryDataSource spendingsDataSource;

    private Color mainColor;
    private string mainCurrency;
    private int currentCategoryId;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        spendingsDataSource = (CategoryDataSource)Resources.Load("CategoryDataSource");
        mainCurrency = PlayerPrefs.GetString("MainCurrency", "$");

        LoadDateTexts();
    }

    public void OpenMenu(int categoryId)
    {
        isMenuToggled = true;

        categoryInfoMenuAnimator.SetBool(IS_MENU_TOGGLED, isMenuToggled);

        currentCategoryId = categoryId;
        categoryNameText.text = spendingsDataSource.lsItems[categoryId].categoryName;

        mainColor = spendingsDataSource.lsItems[categoryId].categoryIconColor;

        dailyMarker.color = mainColor;

        Color weekColor = mainColor;
        weekColor.a = 0.75f;
        weeklyMarker.color = weekColor;

        Color monthColor = mainColor;
        monthColor.a = 0.5f;
        monthlyMarker.color = monthColor;

        dailyText.text = TextColors.ApplyColorToText(mainColor, "ׁüמדמהם³:");
        weeklyText.text = TextColors.ApplyColorToText(mainColor, "ׂטזהוםü:");
        monthlyText.text = TextColors.ApplyColorToText(mainColor, "ּ³סÿצü:");

        SetCategoryId(categoryId);
    }

    private void LoadDateTexts()
    {
        DateTime currentDate = DateTime.Now;
        
        dailyDateText.text = currentDate.ToString("dd.MM");

        DateTime sevenDaysAgo = currentDate.AddDays(-6);
        weeklyDateText.text = sevenDaysAgo.ToString("dd.MM") + "-" + currentDate.ToString("dd.MM");

        monthlyInfoDateText.text = char.ToUpper(currentDate.ToString("MMMM")[0]) + currentDate.ToString("MMMM").Substring(1);
    }

    public void SetCategoryId(int categoryId)
    {
        string categoryName = spendingsDataSource.lsItems[categoryId].categoryName;
        categoryNameText.text = categoryName;

        LoadDailySpendings();
        LoadWeeklySpendings(); 
        LoadMonthlySpendings();
    }

    private void LoadDailySpendings()
    {
        DateTime currentDate = DateTime.Now;
        List<TransactionData> transactionsList = DataManager.Instance.GetHistory();

        float summ = 0f;
        if (transactionsList == null)
        {
            dailyDateText.text = summ.ToString();
        }
        else
        {
            foreach (TransactionData transactionData in transactionsList)
            {
                if (!transactionData.transactionName.Contains(categoryNameText.text))
                    continue;

                if (transactionData.date.Contains(currentDate.ToString("dd.MM.yy")))
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

            dailySumText.text = TextColors.ApplyColorToText(spendingsDataSource.lsItems[currentCategoryId].categoryIconColor, mainCurrency) + "</color>" + doubleVal.ToString().Replace(",", ".");
        }
    }
    
    private void LoadWeeklySpendings()
    {
        DateTime currentDate = DateTime.Now;
        List<TransactionData> transactionsList = DataManager.Instance.GetHistory();

        float summ = 0f;
        if (transactionsList == null)
        {
            weeklySumText.text = summ.ToString();
        }
        else
        {
            foreach (TransactionData transactionData in transactionsList)
            {
                if (!transactionData.transactionName.Contains(categoryNameText.text))
                    continue;

                if (transactionData.date.Contains(currentDate.ToString("dd.MM.yy")) ||
                    transactionData.date.Contains(currentDate.AddDays(-1).ToString("dd.MM.yy")) ||
                    transactionData.date.Contains(currentDate.AddDays(-2).ToString("dd.MM.yy")) ||
                    transactionData.date.Contains(currentDate.AddDays(-3).ToString("dd.MM.yy")) ||
                    transactionData.date.Contains(currentDate.AddDays(-4).ToString("dd.MM.yy")) ||
                    transactionData.date.Contains(currentDate.AddDays(-5).ToString("dd.MM.yy")) ||
                    transactionData.date.Contains(currentDate.AddDays(-6).ToString("dd.MM.yy")))
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

            weeklySumText.text = TextColors.ApplyColorToText(spendingsDataSource.lsItems[currentCategoryId].categoryIconColor, mainCurrency) + "</color>" + doubleVal.ToString().Replace(",", ".");
        }
    }

    private void LoadMonthlySpendings()
    {
        DateTime currentDate = DateTime.Now;
        List<TransactionData> transactionsList = DataManager.Instance.GetHistory();

        float summ = 0f;
        if (transactionsList == null)
        {
            monthlySumText.text = summ.ToString();
        }
        else
        {
            foreach (TransactionData transactionData in transactionsList)
            {
                if (!transactionData.transactionName.Contains(categoryNameText.text))
                    continue;

                if (transactionData.date.Contains(currentDate.ToString("MM.yy")))
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

            monthlySumText.text = TextColors.ApplyColorToText(spendingsDataSource.lsItems[currentCategoryId].categoryIconColor, mainCurrency) + "</color>" + doubleVal.ToString().Replace(",", ".");
        }
    }

    public void CloseMenu()
    {
        isMenuToggled = false;

        categoryInfoMenuAnimator.SetBool(IS_MENU_TOGGLED, isMenuToggled);
    }
}
