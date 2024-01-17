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

    // Start is called before the first frame update
    private void Start()
    {
        // Attach events (creating of new card and succesful spending)
        Card.OnCardCreate += Card_OnCardCreate;
        Card.OnCardDelete += Card_OnCardDelete;
        SpendingMenu.instance.OnBalanceUpdate += SpendingMenu_OnBalanceUpdate;
        ProfitMenu.instance.OnBalanceUpdate += ProfitMenu_OnBalanceUpdate;
        EditCardMenu.instance.OnBalanceUpdate += EditCardMenu_OnBalanceUpdate;

        currentDateText.text = (char.ToUpper(DateTime.Now.ToString("MMMM, yyyy")[0]) + DateTime.Now.ToString("MMMM, yyyy").Substring(1));
        
        SetTotalCardBalance();
    }

    private void Card_OnCardCreate(object sender, EventArgs e)
    {
        SetTotalCardBalance();
    }

    private void Card_OnCardDelete(object sender, EventArgs e)
    {
        SetTotalCardBalance();
    }

    private void SpendingMenu_OnBalanceUpdate(object sender, EventArgs e)
    {
        SetTotalCardBalance();
    }

    private void ProfitMenu_OnBalanceUpdate(object sender, EventArgs e)
    {
        SetTotalCardBalance();
    }

    private void EditCardMenu_OnBalanceUpdate(object sender, EventArgs e)
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

    private void OnDisable()
    {
        Card.OnCardCreate -= Card_OnCardCreate;
        Card.OnCardDelete -= Card_OnCardDelete;
        SpendingMenu.instance.OnBalanceUpdate -= SpendingMenu_OnBalanceUpdate;
        ProfitMenu.instance.OnBalanceUpdate -= ProfitMenu_OnBalanceUpdate;
        EditCardMenu.instance.OnBalanceUpdate -= EditCardMenu_OnBalanceUpdate;
    }
}