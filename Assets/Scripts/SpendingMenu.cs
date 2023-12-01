using System;
using System.Data;
using System.Globalization;
using TMPro;
using UnityEngine;

public class SpendingMenu : MonoBehaviour
{
    public static SpendingMenu instance;

    private Animator spendingsMenuAnimator;

    private const string IS_MENU_TOGGLED = "isMenuToggled";
    [SerializeField] private Animator spendingButtonAnimator;

    [SerializeField] private GameObject MainMenuBlackoutGO;
    [SerializeField] private Animator MainMenuBlackoutAnimator;

    [Space]
    [SerializeField] private TextMeshProUGUI InputText;

    [Header("Items for History")]
    [SerializeField] private Transform HistoryContent;
    [SerializeField] private GameObject NewTransactionHistoryPrefab;
    [SerializeField] Sprite test;

    [Header("Choose Card Texts")]
    private int chooseCardId;
    [SerializeField] private TextMeshProUGUI ChooseCardName;
    [SerializeField] private TextMeshProUGUI ChooseCardBalance;

    [Header("Choose Card Menu")]
    [SerializeField] private GameObject CardChooseMenuGO;
    private CardChooseMenu cardChooseMenuScript;

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
        chooseCardId = 0;
        cardChooseMenuScript = CardChooseMenu.instance;
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

        UpdateChoosenCardInfo();
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
        if (InputText.text == "0" || InputText.text == string.Empty)
        {
            return;
        }

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
        float cardBalanceFloat = PlayerPrefs.GetFloat(Card.CARD_BALANCE_FLOAT + chooseCardId, 0f) - float.Parse(InputText.text.Replace('.', ','));
        string cardBalanceText = cardBalanceFloat.ToString();

        Card.CardList[chooseCardId][1] = cardBalanceText.Replace(',', '.');
        PlayerPrefs.SetString(Card.CARD_BALANCE + chooseCardId, Card.CardList[chooseCardId][1]);
        PlayerPrefs.SetFloat(Card.CARD_BALANCE_FLOAT + chooseCardId, cardBalanceFloat);

        OnBalanceUpdate?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateChoosenCardInfo()
    {
        ChooseCardName.text = PlayerPrefs.GetString(Card.CARD_NAME + chooseCardId.ToString(), "Картка Універсальна");
        ChooseCardBalance.text = "<color=#5EDEA9>" + PlayerPrefs.GetString(Card.CARD_CURRENCY + chooseCardId.ToString(), "$") + "</color>" +
            "<color=#F6F6F6>" + PlayerPrefs.GetString(Card.CARD_BALANCE + chooseCardId.ToString(), "0.00");
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

    public void OpenCardChooseMenu()
    {
        CardChooseMenuGO.SetActive(true);
        cardChooseMenuScript.OpenMenu();
    }

    public void UpdateCardId(int newChooseCardId)
    {
        chooseCardId = newChooseCardId;
        UpdateChoosenCardInfo();
    }
}
