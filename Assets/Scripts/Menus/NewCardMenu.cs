using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewCardMenu : MonoBehaviour
{
    // Menu animations variables
    private Animator newCardMenuAnimator;
    private const string IS_TOGGLED = "isToggled";
    private bool isToggled = false;

    [Header("Animators")]
    [SerializeField] private Animator NewCardNameInputAC;
    [SerializeField] private Animator NewCardBalanceInputAC;
    [SerializeField] private Animator NewCardCurrencyChooseAC;
    [SerializeField] private Animator NewCardPreviewAC;
    private const string IS_NAME_SET = "isNameSet";
    private const string IS_BALANCE_SET = "isBalanceSet";
    private const string CHOOSED_CURRENCY_ID = "ChoosedCurrencyID";

    [Header("Preview")]
    [SerializeField] private TextMeshProUGUI CardNameText;
    [SerializeField] private TextMeshProUGUI CardBalanceText;

    [Header("Confirm Button")]
    [SerializeField] private Button confirmButton;

    private string cardCurrency;
    private string cardCurrencyCode;
    private string cardBalance;

    private bool isNameSet = false;
    private bool isBalanceSet = true;

    private void Awake()
    {
        newCardMenuAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        cardCurrency = "$";
        SetCardBalance(string.Empty);
    }

    private void Update()
    {
        NewCardNameInputAC.SetBool(IS_NAME_SET, isNameSet);
        NewCardBalanceInputAC.SetBool(IS_BALANCE_SET, isBalanceSet);
        NewCardPreviewAC.SetBool(IS_NAME_SET, isNameSet);
        NewCardPreviewAC.SetBool(IS_BALANCE_SET, isBalanceSet);

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
        NewCardCurrencyChooseAC.SetInteger(CHOOSED_CURRENCY_ID, ChoosedCurrencyID);
    }

    public void CreateNewCard()
    {
        // Замість заміни символів, краще використовувати культурні налаштування для парсингу
        var culture = CultureInfo.InvariantCulture; // або інша культура, що використовує кому як десятковий роздільник

        // Парсинг cardBalance з урахуванням культури, яка має кому як десятковий роздільник
        if (float.TryParse(cardBalance, NumberStyles.Any, culture, out float CardBalanceFloat))
        {
            new Card(CardNameText.text, cardBalance, cardCurrency, CardBalanceFloat, cardCurrencyCode);
        }
        else
        {
            // Обробка помилки, якщо парсинг не вдається
            Debug.Log("Невдале перетворення: некоректний формат числа.");
        }

        ToggleNewCardMenu();
    }

    public void ToggleNewCardMenu()
    {
        isToggled = !isToggled;

        newCardMenuAnimator.SetBool(IS_TOGGLED, isToggled);
    }

    public void CloseMenu()
    {
        isToggled = false;

        newCardMenuAnimator.SetBool(IS_TOGGLED, isToggled);
    }
}