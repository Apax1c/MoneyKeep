using System;
using System.Data;
using System.Globalization;
using TMPro;
using UnityEngine;

public class SpendingMenu : MonoBehaviour
{
    public static SpendingMenu instance;

    private Animator spendingsMenuAnimator;

    [SerializeField] private Animator spendingButtonAnimator;
    private const string IS_MENU_TOGGLED = "isMenuToggled";

    [SerializeField] private GameObject MainMenuBlackoutGO;
    [SerializeField] private Animator MainMenuBlackoutAnimator;

    [Space]
    [SerializeField] private TextMeshProUGUI InputText;

    [Header("Items for History")]
    [SerializeField] private Transform HistoryContent;
    [SerializeField] private GameObject NewTransactionHistoryPrefab;
    [SerializeField] Sprite test;

    [Header("Choose Card Texts")]
    private int ChooseCardId;
    [SerializeField] private TextMeshProUGUI ChooseCardName;
    [SerializeField] private TextMeshProUGUI ChooseCardBalance;
    [SerializeField] private TextMeshProUGUI ChooseCardCurrency;

    private bool isMenuToggled = false;

    public event EventHandler OnBalanceUpdate;

    private void Awake()
    {
        instance = this;

        spendingsMenuAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void ToggleSpendingsMenu()
    {
        isMenuToggled = !isMenuToggled;
        spendingButtonAnimator.SetBool(IS_MENU_TOGGLED, isMenuToggled);
        spendingsMenuAnimator.SetBool(IS_MENU_TOGGLED, isMenuToggled);
        MainMenuBlackoutAnimator.SetBool(IS_MENU_TOGGLED, isMenuToggled);
        
        if (isMenuToggled)
        {
            InputText.text = "0";
        }

        ChooseCardId = 0;
        ChooseCardName.text = PlayerPrefs.GetString("CardName0", "Картка Універсальна");
        ChooseCardBalance.text = PlayerPrefs.GetString("CardBalance0", "0.00");
        ChooseCardCurrency.text = PlayerPrefs.GetString("CardCurrency0", "$");
    }

    public void NewNumber(string numberValue)
    {
        if (InputText.text != "0")
        {
            InputText.text += numberValue;
        }
        else
        {
            InputText.text = numberValue;
        }
    }

    public void NewMathOperand(string newOperand)
    {
        if (InputText.text.EndsWith("+") || InputText.text.EndsWith("-") || InputText.text.EndsWith("*") || InputText.text.EndsWith("/"))
        {
            InputText.text = InputText.text.Remove(InputText.text.Length - 1, 1);
        }

        NewNumber(newOperand);
    }

    public void Backspace()
    {
        if (InputText.text != string.Empty)
        {
            InputText.text = InputText.text.Remove(InputText.text.Length - 1, 1);
        }
    }

    public void Equals()
    {
        // Create a DataTable to evaluate the expression
        DataTable dt = new DataTable();

        // Use the Compute method to evaluate the expression
        object result = dt.Compute(InputText.text, "");

        // Convert the result to a float
        if (result != null)
        {
            float floatValue;
            if (float.TryParse(result.ToString(), out floatValue))
            {
                InputText.text = floatValue.ToString();
                InputText.text = InputText.text.Replace(",", ".");
            }
            else
            {
                Debug.Log("Unable to convert result to float.");
            }
        }
        else
        {
            Debug.Log("Expression did not return a valid result.");
        }
    }

    public void OnConfirmClick()
    {
        CreateNewHistoryItem();
        UpdateCardBalance();
    }

    private void UpdateCardBalance()
    {
        Card.LoadCardList();
        float cardBalanceFloat = PlayerPrefs.GetFloat(Card.CARD_BALANCE_FLOAT + ChooseCardId, 0f) - float.Parse(InputText.text.Replace('.', ','));
        string cardBalanceText = cardBalanceFloat.ToString();

        Card.CardList[ChooseCardId][1] = cardBalanceText.Replace(',', '.');
        PlayerPrefs.SetString(Card.CARD_BALANCE + ChooseCardId, Card.CardList[ChooseCardId][1]);
        PlayerPrefs.SetFloat(Card.CARD_BALANCE_FLOAT + ChooseCardId, cardBalanceFloat);

        OnBalanceUpdate?.Invoke(this, EventArgs.Empty);
    }

    private void CreateNewHistoryItem()
    {
        if(InputText.text.Contains("+") || InputText.text.Contains("-") || InputText.text.Contains("*") || InputText.text.Contains("/"))
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

        GameObject newItemGO = Instantiate(NewTransactionHistoryPrefab, HistoryContent);
        TransactionHistoryItem newItem = newItemGO.GetComponent<TransactionHistoryItem>();
        newItem.TransactionHistory("Test", ChooseCardName.text, "-" + InputText.text, test);

        ToggleSpendingsMenu();
    }
}
