using System;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    // DataList of cards
    public static List<string[]> CardList = new List<string[]>();

    // PlayerPrefs keys
    public const string NUMBER_OF_CARDS = "NumberOfCards";
    public const string CARD_NAME = "CardName";
    public const string CARD_BALANCE = "CardBalance";
    public const string CARD_BALANCE_FLOAT = "CardBalanceFloat";
    public const string CARD_CURRENCY = "CardCurrency";

    // Event
    public static event EventHandler OnCardCreate;

    /// <summary>
    /// Initializes new Card and saves data to PlayerPrefs
    /// </summary>
    /// <param name="name">Card name in string</param>
    /// <param name="balance">Card balance in string</param>
    /// <param name="currency">Card currency in string</param>
    /// <param name="balanceFloat">Card balance in float</param>
    public Card(string name, string balance, string currency, float balanceFloat)
    {
        LoadCardList();

        // Add data of new card
        CardList.Add(new string[] { name, balance, currency });
        PlayerPrefs.SetString(CARD_NAME + (CardList.Count - 1).ToString(), name);
        PlayerPrefs.SetString(CARD_BALANCE + (CardList.Count - 1).ToString(), balance);
        PlayerPrefs.SetString(CARD_CURRENCY + (CardList.Count - 1).ToString(), currency);

        PlayerPrefs.SetFloat(CARD_BALANCE_FLOAT + (CardList.Count - 1), balanceFloat);

        // Set count of card
        PlayerPrefs.SetInt(NUMBER_OF_CARDS, CardList.Count);

        // Send event that new card was created
        OnCardCreate?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Loads Card Data (CardList) from PlayerPrefs in List<string> format
    /// </summary>
    public static void LoadCardList()
    {
        int CardListCount = 0;

        // Insert data to CardList while items in list less than count in PlayerPrefs
        while (CardList.Count < PlayerPrefs.GetInt(NUMBER_OF_CARDS, 0))
        {
            // Add items to CardList (name, balance and currency)
            CardList.Add(new string[]
            {   PlayerPrefs.GetString(CARD_NAME + CardListCount.ToString()),
                PlayerPrefs.GetString(CARD_BALANCE + CardListCount.ToString()),
                PlayerPrefs.GetString(CARD_CURRENCY + CardListCount.ToString())
            });

            CardListCount++;
        }
    }
}