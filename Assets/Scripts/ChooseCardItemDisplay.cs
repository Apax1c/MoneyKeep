using TMPro;
using UnityEngine;

public class ChooseCardItemDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardBalanceText;

    private CardChooseMenu cardChooseMenuScript;
    private SpendingMenu spendingsMenuScript;

    private int cardId;

    private void Start()
    {
        spendingsMenuScript = SpendingMenu.instance;
        cardChooseMenuScript = CardChooseMenu.instance;
    }

    public void SetCardId(int newCardId)
    {
        cardId = newCardId;
        SetCardInfo();
    }

    private void OnMouseUpAsButton()
    {
        spendingsMenuScript.UpdateCardId(cardId);
        cardChooseMenuScript.CloseMenu();
    }

    private void SetCardInfo()
    {
        Card.LoadCardList();

        cardNameText.text = Card.CardList[cardId][0];
        cardBalanceText.text = "<color=#5EDEA9>" + Card.CardList[cardId][2] + "</color><color=#F6F6F6>" + Card.CardList[cardId][1];
    }
}
