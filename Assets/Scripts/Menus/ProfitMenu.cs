using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class ProfitMenu : TransactionMenus
{
    public static ProfitMenu instance;

    [Header("Choose Card Menu")]
    [SerializeField] private GameObject CardChooseMenuGO;
    private ProfitCardChooseMenu cardChooseMenuScript;

    [Header("Category Choose Menu")]
    [SerializeField] private GameObject CategoryChooseMenuGO;
    private ProfitCategoryChooseMenu categoryChooseMenuScript;
    public event EventHandler OnBalanceUpdate;

    private void Awake()
    {
        instance = this;

        spendingsMenuAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        this.gameObject.SetActive(false);
        chooseCardId = 0;
        cardChooseMenuScript = ProfitCardChooseMenu.instance;
        categoryChooseMenuScript = ProfitCategoryChooseMenu.instance;

        categoryDataSource = (CategoryDataSource)Resources.Load("ProfitCategoryDataSource");
        UpdateCategoryId(0);
    }

    public void OnConfirmClick()
    {
        CreateNewHistoryItem();
        UpdateCardBalance();
    }

    private void UpdateCardBalance()
    {
        Card.LoadCardList();
        float cardBalanceFloat = PlayerPrefs.GetFloat(Card.CARD_BALANCE_FLOAT + chooseCardId, 0f) + float.Parse(InputText.text.Replace('.', ','));
        string cardBalanceText = cardBalanceFloat.ToString();

        Card.CardList[chooseCardId][1] = cardBalanceText.Replace(',', '.');
        PlayerPrefs.SetString(Card.CARD_BALANCE + chooseCardId, Card.CardList[chooseCardId][1]);
        PlayerPrefs.SetFloat(Card.CARD_BALANCE_FLOAT + chooseCardId, cardBalanceFloat);

        OnBalanceUpdate?.Invoke(this, EventArgs.Empty);
    }

    private void CreateNewHistoryItem()
    {
        if (InputText.text.Contains("+") || InputText.text.Contains("-") || InputText.text.Contains("*") || InputText.text.Contains("/"))
        {
            Equals();
        }

        NumberFormatInfo provider = new NumberFormatInfo();
        provider.NumberDecimalSeparator = ".";
        provider.NumberGroupSeparator = ",";

        double doubleVal = Convert.ToDouble(InputText.text, provider);
        doubleVal = Math.Round(doubleVal, 2);
        InputText.text = doubleVal.ToString(provider);
        InputText.text = InputText.text.Replace(",", ".");

        InstatiateNewDateItem();
        InstantiateHistoryItem();
        SaveTransactionData();

        ToggleMenu();
    }

    private void InstatiateNewDateItem()
    {
        List<TransactionData> list = DataManager.Instance.GetHistory();
        if (list != null && char.ToUpper(DateTime.Now.ToString("dddd, " + "dd.MM.yy")[0]) + DateTime.Now.ToString("dddd, " + "dd.MM.yy").Substring(1) != list[list.Count - 1].date)
        {
            GameObject newDateItemGO = Instantiate(NewDateItemPrefab, HistoryContent);
            newDateItemGO.transform.SetAsFirstSibling();

            DateHistoryItem newDateItem = newDateItemGO.GetComponent<DateHistoryItem>();
            newDateItem.SetDateText(char.ToUpper(DateTime.Now.ToString("dddd, " + "dd.MM.yy")[0]) + DateTime.Now.ToString("dddd, " + "dd.MM.yy").Substring(1));
        }
        else if (list == null)
        {
            GameObject newDateItemGO = Instantiate(NewDateItemPrefab, HistoryContent);
            newDateItemGO.transform.SetAsFirstSibling();

            DateHistoryItem newDateItem = newDateItemGO.GetComponent<DateHistoryItem>();
            newDateItem.SetDateText(char.ToUpper(DateTime.Now.ToString("dddd, " + "dd.MM.yy")[0]) + DateTime.Now.ToString("dddd, " + "dd.MM.yy").Substring(1));
        }
    }

    private void SaveTransactionData()
    {
        TransactionData newTransactionData = new TransactionData(
                    categoryDataSource.lsItems[categoryId].categoryName + transactionComment,
                    ChooseCardName.text,
                    TextColors.ApplyColorToText(TextColors.DefaultColorsEnum.Green, "+" + InputText.text),
                    categoryId,
                    char.ToUpper(DateTime.Now.ToString("dddd, " + "dd.MM.yy")[0]) + DateTime.Now.ToString("dddd, " + "dd.MM.yy").Substring(1),
                    CurrencyText.text,
                    Card.CardList[chooseCardId][3]
                    );
        DataManager.Instance.AddOrUpdateTransaction(newTransactionData);
    }

    private void InstantiateHistoryItem()
    {
        GameObject newItemGO = Instantiate(NewTransactionHistoryPrefab, HistoryContent);
        newItemGO.transform.SetSiblingIndex(1);
        TransactionHistoryItem newItem = newItemGO.GetComponent<TransactionHistoryItem>();
        newItem.TransactionHistory(
            categoryDataSource.lsItems[categoryId].categoryName + transactionComment,
            ChooseCardName.text,
            TextColors.ApplyColorToText(TextColors.DefaultColorsEnum.Green, "+" + InputText.text),
            categoryDataSource.lsItems[categoryId].categoryIcon,
            categoryDataSource.lsItems[categoryId].categoryColor,
            categoryDataSource.lsItems[categoryId].categoryIconColor);
    }

    public void OpenCardChooseMenu()
    {
        CardChooseMenuGO.SetActive(true);
        cardChooseMenuScript.OpenMenu();
    }

    public void OpenCategoryChooseMenu()
    {
        CategoryChooseMenuGO.SetActive(true);
        categoryChooseMenuScript.OpenMenu();
    }

    public void UpdateCardId(int newChooseCardId)
    {
        chooseCardId = newChooseCardId;
        UpdateChoosenCardInfo();
    }

    public void UpdateCategoryId(int newCategoryId, string comment = null)
    {
        categoryId = newCategoryId;

        categoryPreviewBackground.color = categoryDataSource.lsItems[categoryId].categoryColor;
        categoryPreviewIcon.sprite = categoryDataSource.lsItems[categoryId].categoryIcon;
        categoryPreviewIcon.color = categoryDataSource.lsItems[categoryId].categoryIconColor;
        categoryPreviewName.text = Localisation.GetString(categoryDataSource.lsItems[categoryId].categoryName, this);
        categoryPreviewName.color = categoryDataSource.lsItems[categoryId].categoryIconColor;

        if (comment != null)
        {
            transactionComment = ", " + comment;
        }
        else
        {
            transactionComment = string.Empty;
        }
    }
}
