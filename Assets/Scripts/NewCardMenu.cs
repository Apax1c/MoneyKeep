using System;
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
    [SerializeField] private TextMeshProUGUI ConfirmText;
    [SerializeField] private Transform ConfirmIcon;
    [SerializeField] private Button confirmButton;

    private string cardCurrency;
    private string cardBalance;

    private bool isNameSet = false;
    private bool isBalanceSet = true;

    private string wrongBalanceWarning = "Введіть числове значення";

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
        ConfirmIcon.localPosition = new Vector3((ConfirmText.preferredWidth / 2) + 12.4f - 21f + 13.3f/2, 0, 0);

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
        // Замена точек на запятые
        this.cardBalance = cardBalance.Replace(".", ",");

        if (double.TryParse(this.cardBalance, out double result))
        {
            this.cardBalance = result.ToString();

            // Обрезание значение до двух после запятой
            double doubleVal = Convert.ToDouble(this.cardBalance);
            doubleVal = Math.Round(doubleVal, 2);
            this.cardBalance = doubleVal.ToString();

            // Добавляет нули в конце, если других значений нет
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
            this.cardBalance = wrongBalanceWarning;

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

    public void ToggleCurrencyAnimation(int ChoosedCurrencyID)
    {
        NewCardCurrencyChooseAC.SetInteger(CHOOSED_CURRENCY_ID, ChoosedCurrencyID);
    }

    public void CreateNewCard()
    {
        float CardBalanceFloat = float.Parse(cardBalance.Replace('.', ','));

        new Card(CardNameText.text, cardBalance, cardCurrency, CardBalanceFloat);

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
