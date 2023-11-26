using System;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentDateText;
    [SerializeField] private TextMeshProUGUI totalBalanceText;

    [SerializeField] private GameObject NewCardMenu;

    private float totalBalance;
    private int balanceIdInList = 1;

    private void Awake()
    {
        // Attach events (creating of new card and succesful spending)
        Card.OnCardCreate += Card_OnCardCreate;
        SpendingMenu.instance.OnBalanceUpdate += SpendingMenu_OnBalanceUpdate;
    }

    // Start is called before the first frame update
    private void Start()
    {
        currentDateText.text = (char.ToUpper(DateTime.Now.ToString("MMMM, yyyy")[0]) + DateTime.Now.ToString("MMMM, yyyy").Substring(1));
        
        SetTotalCardBalance();
    }

    private void Card_OnCardCreate(object sender, EventArgs e)
    {
        SetTotalCardBalance();
    }

    private void SpendingMenu_OnBalanceUpdate(object sender, EventArgs e)
    {
        SetTotalCardBalance();
    }

    private void SetTotalCardBalance()
    {
        totalBalance = 0;

        Card.LoadCardList();

        foreach (string[] card in Card.CardList)
        {
            totalBalance += float.Parse(card[balanceIdInList].Replace(".", ","));
        }

        totalBalanceText.text = totalBalance.ToString();
    }
}