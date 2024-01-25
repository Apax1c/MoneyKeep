using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditCardMenu : MonoBehaviour
{
    public static EditCardMenu instance;

    // Menu animations variables
    private Animator editCardMenuAnimator;
    private const string IS_TOGGLED = "isToggled";
    private bool isToggled = false;

    [Header("Animators")]
    [SerializeField] private Animator EditCardNameInputAC;
    [SerializeField] private Animator EditCardBalanceInputAC;
    [SerializeField] private Animator EditCardCurrencyChooseAC;
    [SerializeField] private Animator EditCardPreviewAC;
    private const string IS_NAME_SET = "isNameSet";
    private const string IS_BALANCE_SET = "isBalanceSet";
    private const string CHOOSED_CURRENCY_ID = "ChoosedCurrencyID";

    [Header("Preview")]
    [SerializeField] private TextMeshProUGUI CardNameText;
    [SerializeField] private TextMeshProUGUI CardBalanceText;

    [Header("InputField")]
    [SerializeField] private TMP_InputField cardNameInput;
    [SerializeField] private TMP_InputField cardBalanceInput;

    [Header("Confirm Button")]
    [SerializeField] private Button confirmButton;

    [Space]
    [SerializeField] private CardBehaviour cardBehaviour;

    private string cardCurrency;
    private string cardCurrencyCode;
    private string cardBalance;

    private bool isNameSet = false;
    private bool isBalanceSet = true;

    public event EventHandler OnBalanceUpdate;

    private void Awake()
    {
        instance = this;

        editCardMenuAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        LoadCardInfo();
    }

    private void Update()
    {
        EditCardNameInputAC.SetBool(IS_NAME_SET, isNameSet);
        EditCardBalanceInputAC.SetBool(IS_BALANCE_SET, isBalanceSet);
        EditCardPreviewAC.SetBool(IS_NAME_SET, isNameSet);
        EditCardPreviewAC.SetBool(IS_BALANCE_SET, isBalanceSet);

        if (isNameSet && isBalanceSet)
        {
            confirmButton.interactable = true;
        }
        else
        {
            confirmButton.interactable = false;
        }
    }

    public void SetCardName(string cardName)
    {
        CardNameText.text = cardName;
        isNameSet = true;

        if (cardName == string.Empty)
        {
            isNameSet = false;
        }
    }

    public void SetCardBalance(string cardBalance)
    {
        // «амена точек на зап€тые
        this.cardBalance = cardBalance.Replace(".", ",");

        if (double.TryParse(this.cardBalance, out double result))
        {
            this.cardBalance = result.ToString();

            // ќбрезание значение до двух после зап€той
            double doubleVal = Convert.ToDouble(this.cardBalance);
            doubleVal = Math.Round(doubleVal, 2);
            this.cardBalance = doubleVal.ToString();

            // ƒобавл€ет нули в конце, если других значений нет
            if (!this.cardBalance.Contains(","))
            {
                this.cardBalance += ",00";
            }
            else if (this.cardBalance.Length == (this.cardBalance.IndexOf(',') + 2))
            {
                this.cardBalance += "0";
            }

            this.cardBalance = this.cardBalance.Replace(",", ".");

            isBalanceSet = true;
        }
        else if (cardBalance == string.Empty)
        {
            this.cardBalance = "0.00";

            isBalanceSet = true;
        }
        else
        {
            this.cardBalance = Localisation.GetString("EnterNumericValue", this);

            isBalanceSet = false;
        }

        CardBalanceText.text = TextColors.ApplyColorToText(TextColors.DefaultColorsEnum.Green, cardCurrency) + " "
            + TextColors.ApplyColorToText(TextColors.DefaultColorsEnum.White, this.cardBalance);
    }

    public void SetCurrency(string currency)
    {
        cardCurrency = currency;
        SetCardBalance(cardBalance);
    }

    public void SetCurrencyCode(string currencyCode)
    {
        cardCurrencyCode = currencyCode;
    }

    public void ToggleCurrencyAnimation(int ChoosedCurrencyID)
    {
        EditCardCurrencyChooseAC.SetInteger(CHOOSED_CURRENCY_ID, ChoosedCurrencyID);
    }

    public void UpdateCard()
    {
        float CardBalanceFloat = float.Parse(cardBalance.Replace('.', ','));

        PlayerPrefs.SetString(Card.CARD_NAME + cardBehaviour.CardId, CardNameText.text);
        PlayerPrefs.SetString(Card.CARD_BALANCE + cardBehaviour.CardId, cardBalance);
        PlayerPrefs.SetString(Card.CARD_CURRENCY + cardBehaviour.CardId, cardCurrency);
        PlayerPrefs.SetFloat(Card.CARD_BALANCE_FLOAT + cardBehaviour.CardId, CardBalanceFloat);

        Card.UpdateCardList(cardBehaviour.CardId, CardNameText.text, cardBalance, cardCurrency, cardCurrencyCode);

        ToggleMenu();

        OnBalanceUpdate?.Invoke(this, EventArgs.Empty);
    }

    public void LoadCardInfo()
    {
        Card.LoadCardList();

        cardCurrency = PlayerPrefs.GetString(Card.CARD_CURRENCY + cardBehaviour.CardId, "$");

        SetCardName(PlayerPrefs.GetString(Card.CARD_NAME + cardBehaviour.CardId, Localisation.GetString("DefaultCardName", this)));
        cardNameInput.text = PlayerPrefs.GetString(Card.CARD_NAME + cardBehaviour.CardId, Localisation.GetString("DefaultCardName", this));

        SetCardBalance(PlayerPrefs.GetString(Card.CARD_BALANCE + cardBehaviour.CardId, "0.00"));
        cardBalanceInput.text = PlayerPrefs.GetString(Card.CARD_BALANCE + cardBehaviour.CardId, "0.00");
    }

    public void DeleteCard()
    {
        Card.DeleteCard(cardBehaviour.CardId);

        CloseMenu();
    }

    public void ToggleMenu()
    {
        LoadCardInfo();

        isToggled = !isToggled;

        editCardMenuAnimator.SetBool(IS_TOGGLED, isToggled);
    }

    public void CloseMenu()
    {
        isToggled = false;

        editCardMenuAnimator.SetBool(IS_TOGGLED, isToggled);
    }
}
