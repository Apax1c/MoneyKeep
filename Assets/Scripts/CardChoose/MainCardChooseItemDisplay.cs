using System;
using TMPro;
using UnityEngine;

public class MainCardChooseItemDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardBalanceText;

    [SerializeField] private GameObject cardFrame;

    private MainCardChooseMenu cardChooseMenuScript;

    private int cardId;

    private void Start()
    {
        cardChooseMenuScript = MainCardChooseMenu.instance;
    }

    private void Update()
    {
        if (cardChooseMenuScript.cardId == cardId)
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
        Debug.Log("Item Display: " + cardId);
        cardChooseMenuScript.SetCardId(cardId);
    }

    private void SetCardInfo()
    {
        Card.LoadCardList();

        cardNameText.text = Card.CardList[cardId][0];
        cardBalanceText.text = "<color=#5EDEA9>" + Card.CardList[cardId][2] + "</color><color=#F6F6F6>" + Card.CardList[cardId][1];
    }
}
