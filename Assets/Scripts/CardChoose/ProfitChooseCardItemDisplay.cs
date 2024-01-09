using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfitChooseCardItemDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardBalanceText;

    [SerializeField] private GameObject cardFrame;

    private ProfitCardChooseMenu profitCardChooseMenuScript;

    private int cardId;

    private void Start()
    {
        profitCardChooseMenuScript = ProfitCardChooseMenu.instance;
    }

    private void Update()
    {
        if (profitCardChooseMenuScript.choosedCardId == cardId)
        {
            cardFrame.SetActive(true);
        }
        else
        {
            cardFrame.SetActive(false);
        }
    }

    public void SetCardId(int newCardId)
    {
        cardId = newCardId;
        SetCardInfo();
    }

    public void ChooseCardId()
    {
        profitCardChooseMenuScript.SetCardId(cardId);
    }

    private void SetCardInfo()
    {
        Card.LoadCardList();

        cardNameText.text = Card.CardList[cardId][0];
        cardBalanceText.text = "<color=#5EDEA9>" + Card.CardList[cardId][2] + "</color><color=#F6F6F6>" + Card.CardList[cardId][1];
    }
}
