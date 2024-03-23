using System;
using System.Globalization;
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
        var culture = System.Globalization.CultureInfo.CurrentCulture;
        var decimalSeparator = culture.NumberFormat.NumberDecimalSeparator;

        // Підготовка рядка для конвертації
        string preparedBalance = decimalSeparator == "," ? cardBalance.Replace(".", ",") : cardBalance.Replace(",", ".");

        // Спроба конвертації
        if (double.TryParse(preparedBalance, NumberStyles.Any, culture, out double result))
        {
            // Округлення до двох знаків після коми
            result = Math.Round(result, 2);

            // Форматування рядка з числа з урахуванням локалі
            this.cardBalance = result.ToString("F2", culture);

            isBalanceSet = true;
        }
        else if (string.IsNullOrEmpty(cardBalance))
        {
            this.cardBalance = "0.00";
            isBalanceSet = true;
        }
        else
        {
            this.cardBalance = Localisation.GetString("EnterNumericValue", this);
            isBalanceSet = false;
        }

        // Припустимо, що CardBalanceText - це UI елемент, якому ми встановлюємо текст
        CardBalanceText.text = TextColors.ApplyColorToText(TextColors.DefaultColorsEnum.Green, cardCurrency) + " " +
                               TextColors.ApplyColorToText(TextColors.DefaultColorsEnum.White, this.cardBalance);
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
        // Замість заміни символів, краще використовувати культурні налаштування для парсингу
        var culture = CultureInfo.InvariantCulture; // або інша культура, що використовує кому як десятковий роздільник

        // Парсинг cardBalance з урахуванням культури, яка має кому як десятковий роздільник
        float CardBalanceFloat = float.Parse(cardBalance, NumberStyles.Any, culture);

        PlayerPrefs.SetString(Card.CARD_NAME + cardBehaviour.CardId.ToString(), CardNameText.text);
        PlayerPrefs.SetString(Card.CARD_BALANCE + cardBehaviour.CardId.ToString(), cardBalance);
        PlayerPrefs.SetString(Card.CARD_CURRENCY + cardBehaviour.CardId.ToString(), cardCurrency);
        PlayerPrefs.SetFloat(Card.CARD_BALANCE_FLOAT + cardBehaviour.CardId.ToString(), CardBalanceFloat);

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
