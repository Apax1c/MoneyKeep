using System;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class CategoryInfoMenu : MonoBehaviour
{
    public static CategoryInfoMenu instance;

    private Animator categoryInfoMenuAnimator;
    private const string IS_MENU_TOGGLED = "isMenuToggled";
    private bool isMenuToggled = false;

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

    [Header("SpendingBar")]
    [SerializeField] private Image dailyBarImage;
    [SerializeField] private Slider dailyBarSlider;
    [SerializeField] private Image weeklyBarImage;
    [SerializeField] private Slider weeklyBarSlider;
    [SerializeField] private Image monthlyBarImage;

    [Header("Prefabs")]
    [SerializeField] private GameObject dateItemPrefab;
    [SerializeField] private GameObject transactionItemPrefab;

    [Space]
    [SerializeField] private Transform historyContentGO;
    private string lastItemDate;
    private List<TransactionData> transactions = new List<TransactionData>();

    private CategoryDataSource spendingsDataSource;

    private float dailySum;
    private float weeklySum;
    private float monthlySum;

    private Color mainColor;
    private string mainCurrency;
    private int currentCategoryId;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        categoryInfoMenuAnimator = GetComponent<Animator>();

        spendingsDataSource = (CategoryDataSource)Resources.Load("CategoryDataSource");
        mainCurrency = PlayerPrefs.GetString("MainCurrency", "$");

        LoadDateTexts();
    }

    public void OpenMenu(int categoryId)
    {
        isMenuToggled = true;

        categoryInfoMenuAnimator.SetBool(IS_MENU_TOGGLED, isMenuToggled);

        currentCategoryId = categoryId;
        categoryNameText.text = Localisation.GetString(spendingsDataSource.lsItems[categoryId].categoryName, this);

        mainColor = spendingsDataSource.lsItems[categoryId].categoryIconColor;

        //Daily colors set
        dailyMarker.color = mainColor;
        dailyBarImage.color = mainColor;

        //Week colors set
        Color weekColor = mainColor;
        weekColor.a = 0.75f;
        weeklyMarker.color = weekColor;

        //Month colors set
        Color monthColor = mainColor;
        monthColor.a = 0.5f;
        monthlyMarker.color = monthColor;
        weeklyBarImage.color = monthColor;
        monthlyBarImage.color = monthColor;

        dailyText.text = TextColors.ApplyColorToText(mainColor, Localisation.GetString("Today", this));
        weeklyText.text = TextColors.ApplyColorToText(mainColor, Localisation.GetString("Week", this));
        monthlyText.text = TextColors.ApplyColorToText(mainColor, Localisation.GetString("Month", this));

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
        categoryNameText.text = Localisation.GetString(categoryName, this);

        LoadDailySpendings();
        LoadWeeklySpendings(); 
        LoadMonthlySpendings();
        SetSlidersValue();

        ClearHistory();
        LoadHistory();
    }

    private void ClearHistory()
    {
        if (historyContentGO.transform.childCount != 0)
        {
            for (int i = 0; i < historyContentGO.transform.childCount; i++)
            {
                if (historyContentGO.transform.GetChild(i).gameObject != null)
                {
                    Destroy(historyContentGO.transform.GetChild(i).gameObject);
                }
            }
        }
    }

    private void SetSlidersValue()
    {
        dailyBarSlider.maxValue = monthlySum;
        dailyBarSlider.value = dailySum;

        weeklyBarSlider.maxValue = monthlySum;
        weeklyBarSlider.value = weeklySum;
    }

    private void LoadDailySpendings()
    {
        DateTime currentDate = DateTime.Now;
        List<TransactionData> transactionsList = DataManager.Instance.GetHistory();

        float summ = 0f;
        if (transactionsList == null)
        {
            dailySumText.text = summ.ToString();
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

            dailySum = summ;

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

            weeklySum = summ;

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

            monthlySum = summ;

            double doubleVal = Convert.ToDouble(summ);
            doubleVal = Math.Round(doubleVal, 2);

            monthlySumText.text = TextColors.ApplyColorToText(spendingsDataSource.lsItems[currentCategoryId].categoryIconColor, mainCurrency) + "</color>" + doubleVal.ToString().Replace(",", ".");
        }
    }

    private void LoadHistory()
    {
        transactions = DataManager.Instance.GetHistory();

        if (transactions == null)
            return;
        lastItemDate = transactions[0].date;

        GameObject lastDateItemGO = Instantiate(dateItemPrefab, historyContentGO);
        lastDateItemGO.transform.SetAsFirstSibling();

        DateHistoryItem lastDateItem = lastDateItemGO.GetComponent<DateHistoryItem>();
        lastDateItem.SetDateText(lastItemDate, categoryNameText.text);

        foreach (var transaction in transactions)
        {
            if (!transaction.transactionName.Contains(categoryNameText.text))
                continue;

            if (lastItemDate != transaction.date)
            {
                GameObject newDateItemGO = Instantiate(dateItemPrefab, historyContentGO);
                newDateItemGO.transform.SetAsFirstSibling();

                DateHistoryItem newDateItem = newDateItemGO.GetComponent<DateHistoryItem>();
                newDateItem.SetDateText(transaction.date, categoryNameText.text);

                lastItemDate = transaction.date;
            }

            // Instantiate your objects here and initialize them with the transaction data
            GameObject newItemGO = Instantiate(transactionItemPrefab, historyContentGO);
            newItemGO.transform.SetSiblingIndex(1);
            TransactionHistoryItem newItem = newItemGO.GetComponent<TransactionHistoryItem>();

            newItem.TransactionHistory(
                    transaction.transactionName,
                    transaction.cardName,
                    transaction.transactionSum,
                    spendingsDataSource.lsItems[transaction.details].categoryIcon,
                    spendingsDataSource.lsItems[transaction.details].categoryColor,
                    spendingsDataSource.lsItems[transaction.details].categoryIconColor);
        }
    }

    public void CloseMenu()
    {
        isMenuToggled = false;

        categoryInfoMenuAnimator.SetBool(IS_MENU_TOGGLED, isMenuToggled);
    }
}
