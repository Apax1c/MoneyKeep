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
    [SerializeField] private TextMeshProUGUI CardCurrencyText;

    [Header("Confirm Button")]
    [SerializeField] private TextMeshProUGUI ConfirmText;
    [SerializeField] private Transform ConfirmIcon;
    [SerializeField] private Button confirmButton;

    private bool isNameSet = false;
    private bool isBalanceSet = true;

    private string wrongBalanceWarning = "Введіть числове значення";

    private void Awake()
    {
        newCardMenuAnimator = GetComponent<Animator>();
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
        CardBalanceText.text = cardBalance.Replace(".", ",");

        if (double.TryParse(CardBalanceText.text, out double result))
        {
            CardBalanceText.text = result.ToString();

            // Обрезание значение до двух после запятой
            double doubleVal = Convert.ToDouble(CardBalanceText.text);
            doubleVal = Math.Round(doubleVal, 2);
            CardBalanceText.text = doubleVal.ToString();

            // Добавляет нули в конце, если других значений нет
            if (!CardBalanceText.text.Contains(","))
            {
                CardBalanceText.text += ",00";
            }
            else if (CardBalanceText.text.Length == (CardBalanceText.text.IndexOf(',') + 2))
            {
                CardBalanceText.text += "0";
            }

            CardBalanceText.text = CardBalanceText.text.Replace(",", ".");
            isBalanceSet = true;
        }
        else if (cardBalance == string.Empty)
        {
            CardBalanceText.text = "0.00";
            isBalanceSet = true;
        }
        else
        {
            CardBalanceText.text = wrongBalanceWarning;
            isBalanceSet = false;
        }
    }

    public void SetCurrency(string currency)
    {
        CardCurrencyText.text = currency;
    }

    public void ToggleCurrencyAnimation(int ChoosedCurrencyID)
    {
        NewCardCurrencyChooseAC.SetInteger(CHOOSED_CURRENCY_ID, ChoosedCurrencyID);
    }

    public void CreateNewCard()
    {
        float CardBalanceFloat = float.Parse(CardBalanceText.text.Replace('.', ','));

        new Card(CardNameText.text, CardBalanceText.text, CardCurrencyText.text, CardBalanceFloat);

        ToggleNewCardMenu();
    }

    public void ToggleNewCardMenu()
    {
        isToggled = !isToggled;

        newCardMenuAnimator.SetBool(IS_TOGGLED, isToggled);
    }
}
