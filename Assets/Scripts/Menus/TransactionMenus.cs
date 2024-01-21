using System.Data;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;

public class TransactionMenus : MonoBehaviour
{
    public Animator spendingsMenuAnimator;

    public const string IS_MENU_TOGGLED = "isMenuToggled";
    public bool isMenuToggled = false;
    public Animator menuButtonAnimator;

    public GameObject MainMenuBlackoutGO;
    public Animator MainMenuBlackoutAnimator;

    public TextMeshProUGUI InputText;
    public TextMeshProUGUI CurrencyText;

    [Header("Items for History")]
    public Transform HistoryContent;
    public GameObject NewTransactionHistoryPrefab;
    public GameObject NewDateItemPrefab;
    public string transactionComment;

    public int chooseCardId;
    [Header("Choose Card Texts")]
    public TextMeshProUGUI ChooseCardName;
    public TextMeshProUGUI ChooseCardBalance;

    public int categoryId;
    [Header("Category Preview")]
    public SVGImage categoryPreviewBackground;
    public SVGImage categoryPreviewIcon;
    public TextMeshProUGUI categoryPreviewName;

    public CategoryDataSource categoryDataSource;

    public void ToggleMenu()
    {
        isMenuToggled = !isMenuToggled;
        menuButtonAnimator.SetBool(IS_MENU_TOGGLED, isMenuToggled);
        spendingsMenuAnimator.SetBool(IS_MENU_TOGGLED, isMenuToggled);
        MainMenuBlackoutAnimator.SetBool(IS_MENU_TOGGLED, isMenuToggled);

        if (isMenuToggled)
        {
            InputText.text = "0";
            transactionComment = string.Empty;
        }

        UpdateChoosenCardInfo();
    }

    public void CloseMenu()
    {
        isMenuToggled = false;

        menuButtonAnimator.SetBool(IS_MENU_TOGGLED, isMenuToggled);
        spendingsMenuAnimator.SetBool(IS_MENU_TOGGLED, isMenuToggled);
        MainMenuBlackoutAnimator.SetBool(IS_MENU_TOGGLED, isMenuToggled);

        if (isMenuToggled)
        {
            InputText.text = "0";
            transactionComment = string.Empty;
        }

        UpdateChoosenCardInfo();
    }

    public void UpdateChoosenCardInfo()
    {
        ChooseCardName.text = PlayerPrefs.GetString(Card.CARD_NAME + chooseCardId.ToString(), "Картка Універсальна");
        ChooseCardBalance.text = "<color=#5EDEA9>" + PlayerPrefs.GetString(Card.CARD_CURRENCY + chooseCardId.ToString(), "$") + "</color>" +
            "<color=#F6F6F6>" + PlayerPrefs.GetString(Card.CARD_BALANCE + chooseCardId.ToString(), "0.00");
        CurrencyText.text = "<color=#5EDEA9>" + PlayerPrefs.GetString(Card.CARD_CURRENCY + chooseCardId.ToString(), "$");
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
            if (float.TryParse(result.ToString(), out float floatValue))
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
}
