using System;
using TMPro;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardBalanceText;
    [SerializeField] private TextMeshProUGUI cardCurrencyText;

    private void Awake()
    {
        CardBehaviour cardBehaviour = GetComponent<CardBehaviour>();
        cardBehaviour.OnCardInfoSet += CardBehavior_OnCardInfoSet;
        cardBehaviour.OnCardBalanceUpdate += CardBehaviour_OnCardBalanceUpdate;
    }

    private void CardBehaviour_OnCardBalanceUpdate(object sender, EventArgs e)
    {
        CardBehaviour card = GetComponent<CardBehaviour>();
        cardBalanceText.text = card.CardBalance;
    }

    private void Start()
    {
        CardBehaviour card = GetComponent<CardBehaviour>();
        cardNameText.text = card.CardName;
        cardBalanceText.text = card.CardBalance;
        cardCurrencyText.text = card.CardCurrency;
    }

    private void CardBehavior_OnCardInfoSet(object sender, EventArgs e)
    {
        CardBehaviour card = GetComponent<CardBehaviour>();
        cardNameText.text = card.CardName;
        cardBalanceText.text = card.CardBalance;
        cardCurrencyText.text = card.CardCurrency;
    }
}