using System;
using System.Collections.Generic;
using System.Xml.Linq;
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
    public static event EventHandler OnCardDelete;

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
        PlayerPrefs.Save();

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
            {   
                PlayerPrefs.GetString(CARD_NAME + CardListCount.ToString()),
                PlayerPrefs.GetString(CARD_BALANCE + CardListCount.ToString()),
                PlayerPrefs.GetString(CARD_CURRENCY + CardListCount.ToString())
            });

            CardListCount++;
        }
    }

    public static void UpdateCardList(int cardId, string name, string balance, string currency)
    {
        CardList[cardId][0] = name;
        CardList[cardId][1] = balance;
        CardList[cardId][2] = currency;

        PlayerPrefs.Save();
    }

    public static void DeleteCard(int cardId)
    {
        UnityEngine.Debug.Log("Delete Process");
        UnityEngine.Debug.Log("Name: " + CardList[cardId][0] + "; Balance: " + CardList[cardId][1]);

        CardList.RemoveAt(cardId);
        int CardListCount = 0;

        PlayerPrefs.DeleteAll();

        if (CardList.Count == 0)
        {
            OnCardDelete?.Invoke(EditCardMenu.instance, EventArgs.Empty);
            return;
        }

        while (CardListCount < CardList.Count)
        {
            PlayerPrefs.SetString(CARD_NAME + (CardListCount).ToString(), CardList[CardListCount][0]);
            PlayerPrefs.SetString(CARD_BALANCE + (CardListCount).ToString(), CardList[CardListCount][1]);
            PlayerPrefs.SetString(CARD_CURRENCY + (CardListCount).ToString(), CardList[CardListCount][2]);

            // Set count of card
            PlayerPrefs.SetInt(NUMBER_OF_CARDS, CardList.Count);

            CardListCount++;
        }
        PlayerPrefs.Save();

        OnCardDelete?.Invoke(EditCardMenu.instance, EventArgs.Empty);
    }
}