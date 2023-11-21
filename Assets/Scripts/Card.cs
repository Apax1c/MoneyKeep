using System;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public static List<string[]> CardList = new List<string[]>();

    public const string NUMBER_OF_CARDS = "NumberOfCards";
    public const string CARD_NAME = "CardName";
    public const string CARD_BALANCE = "CardBalance";
    public const string CARD_BALANCE_FLOAT = "CardBalanceFloat";
    public const string CARD_CURRENCY = "CardCurrency";

    public static event EventHandler OnCardCreate;

    public Card(string name, string balance, string currency, float balanceFloat)
    {
        LoadCardList();

        CardList.Add(new string[] { name, balance, currency });
        PlayerPrefs.SetString(CARD_NAME + (CardList.Count - 1).ToString(), name);
        PlayerPrefs.SetString(CARD_BALANCE + (CardList.Count - 1).ToString(), balance);
        PlayerPrefs.SetString(CARD_CURRENCY + (CardList.Count - 1).ToString(), currency);

        PlayerPrefs.SetFloat(CARD_BALANCE_FLOAT + (CardList.Count - 1), balanceFloat);

        PlayerPrefs.SetInt(NUMBER_OF_CARDS, CardList.Count);

        OnCardCreate?.Invoke(this, EventArgs.Empty);
    }

    public static void LoadCardList()
    {
        int CardListCount = 0;

        while (CardList.Count < PlayerPrefs.GetInt(NUMBER_OF_CARDS, 0))
        {
            CardList.Add(new string[]
            {   PlayerPrefs.GetString(CARD_NAME + CardListCount.ToString()),
                PlayerPrefs.GetString(CARD_BALANCE + CardListCount.ToString()),
                PlayerPrefs.GetString(CARD_CURRENCY + CardListCount.ToString())
            });

            CardListCount++;
        }
    }
}