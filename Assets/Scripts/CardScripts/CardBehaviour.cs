using System;
using UnityEngine;

public class CardBehaviour : MonoBehaviour
{
    // Card variables
    public int CardId { get; private set; }
    public string CardName { get; private set; }
    public string CardBalance { get; private set; }
    public string CardCurrency { get; private set; }

    // Events
    public event EventHandler OnCardInfoSet;
    public event EventHandler OnCardBalanceUpdate;

    private void Awake()
    {
        CardId = 0;

        // Set info of Main card
        CardName = PlayerPrefs.GetString("CardName0", Localisation.GetString("DefaultCardName", this));
        CardBalance = PlayerPrefs.GetString("CardBalance0", "0.00");
        CardCurrency = PlayerPrefs.GetString("CardCurrency0", "$");
    }

    private void Start()
    {
        // Attach events (creating of new card and succesful spending)
        Card.OnCardCreate += Card_OnCardCreate;
        Card.OnCardDelete += Card_OnCardDelete;
        SpendingMenu.instance.OnBalanceUpdate += SpendingMenu_OnBalanceUpdate;
        ProfitMenu.instance.OnBalanceUpdate += ProfitMenu_OnBalanceUpdate;
        EditCardMenu.instance.OnBalanceUpdate += EditCardMenu_OnCardUpdate;
        MainCardChooseMenu.instance.OnMainCardChoosed += MainCardChooseMenu_OnMainCardChoosed;
    }

    private void MainCardChooseMenu_OnMainCardChoosed(object sender, EventArgs e)
    {
        CardId = MainCardChooseMenu.instance.cardId;

        Card.LoadCardList();

        CardName = Card.CardList[CardId][0];
        CardBalance = Card.CardList[CardId][1];
        CardCurrency = Card.CardList[CardId][2];

        OnCardInfoSet?.Invoke(this, EventArgs.Empty);
    }

    private void SpendingMenu_OnBalanceUpdate(object sender, EventArgs e)
    {
        CardBalance = Card.CardList[CardId][1];

        OnCardBalanceUpdate?.Invoke(this, EventArgs.Empty);
    }

    private void ProfitMenu_OnBalanceUpdate(object sender, EventArgs e)
    {
        CardBalance = Card.CardList[CardId][1];

        OnCardBalanceUpdate?.Invoke(this, EventArgs.Empty);
    }

    private void EditCardMenu_OnCardUpdate(object sender, EventArgs e)
    {
        Card.LoadCardList();

        CardName = Card.CardList[CardId][0];
        CardBalance = Card.CardList[CardId][1];
        CardCurrency = Card.CardList[CardId][2];

        OnCardInfoSet?.Invoke(this, EventArgs.Empty);
    }

    private void Card_OnCardCreate(object sender, EventArgs e)
    {
        // If card is first it sets on th main card
        if(Card.CardList.Count > 0)
        {
            CardName = Card.CardList[CardId][0];
            CardBalance = Card.CardList[CardId][1];
            CardCurrency = Card.CardList[CardId][2];

            OnCardInfoSet?.Invoke(this, EventArgs.Empty);
        }
    }
    
    private void Card_OnCardDelete(object sender, EventArgs e)
    {
        // If card is first it sets on th main card
        if(Card.CardList.Count - 1 >= CardId)
        {
            CardName = Card.CardList[CardId][0];
            CardBalance = Card.CardList[CardId][1];
            CardCurrency = Card.CardList[CardId][2];

            OnCardInfoSet?.Invoke(this, EventArgs.Empty);
        }
        else if (Card.CardList.Count - 1 < CardId && Card.CardList.Count > 0)
        {
            CardId = 0;

            CardName = Card.CardList[CardId][0];
            CardBalance = Card.CardList[CardId][1];
            CardCurrency = Card.CardList[CardId][2];

            OnCardInfoSet?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            CardName = PlayerPrefs.GetString("CardName0", Localisation.GetString("DefaultCardName", this));
            CardBalance = PlayerPrefs.GetString("CardBalance0", "0.00");
            CardCurrency = PlayerPrefs.GetString("CardCurrency0", "$");

            OnCardInfoSet?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnDisable()
    {
        Card.OnCardCreate -= Card_OnCardCreate;
        Card.OnCardDelete -= Card_OnCardDelete;
        SpendingMenu.instance.OnBalanceUpdate -= SpendingMenu_OnBalanceUpdate;
        ProfitMenu.instance.OnBalanceUpdate -= ProfitMenu_OnBalanceUpdate;
        EditCardMenu.instance.OnBalanceUpdate -= EditCardMenu_OnCardUpdate;
        MainCardChooseMenu.instance.OnMainCardChoosed -= MainCardChooseMenu_OnMainCardChoosed;
    }
}