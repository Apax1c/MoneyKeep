using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfitCardChooseMenu : MonoBehaviour
{
    // Singleton
    public static ProfitCardChooseMenu instance;

    // GameObjects
    [SerializeField] private GameObject ChooseCardItemPrefab;
    [SerializeField] private GameObject contentGO;

    private ProfitMenu profitMenuScript;

    private Animator chooseCardAnimator;

    private const string IS_MENU_TOGGLED = "isMenuToggled";

    // Variables
    private float contentItemsHeight = 0;
    private bool isMenuToggled = false;
    public int choosedCardId { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        profitMenuScript = ProfitMenu.instance;
        choosedCardId = 0;

        chooseCardAnimator = GetComponent<Animator>();
        LoadChooseCardItems();
        gameObject.SetActive(false);
    }

    private void LoadChooseCardItems()
    {
        Card.LoadCardList();

        if (contentGO.transform.childCount != 0)
        {
            for (int i = 0; i < contentGO.transform.childCount; i++)
            {
                if (contentGO.transform.GetChild(i).gameObject != null)
                {
                    Destroy(contentGO.transform.GetChild(i).gameObject);
                }
            }
        }

        for (int i = 0; i < Card.CardList.Count; i++)
        {
            GameObject newChooseCardItem = Instantiate(ChooseCardItemPrefab, contentGO.transform);
            newChooseCardItem.GetComponent<ProfitChooseCardItemDisplay>().SetCardId(i);
        }

        UpdateContentSize();
    }

    public void SetCardId(int newCardId)
    {
        choosedCardId = newCardId;
    }

    public void ConfirmCard()
    {
        profitMenuScript.UpdateCardId(choosedCardId);
        CloseMenu();
    }

    public void OpenMenu()
    {
        isMenuToggled = true;
        chooseCardAnimator.SetBool(IS_MENU_TOGGLED, isMenuToggled);
        LoadChooseCardItems();
    }

    public void CloseMenu()
    {
        isMenuToggled = false;
        chooseCardAnimator.SetBool(IS_MENU_TOGGLED, isMenuToggled);
    }

    private void UpdateContentSize()
    {
        contentItemsHeight = 0;

        for (int i = 0; i < Card.CardList.Count; i++)
        {
            RectTransform child = (RectTransform)contentGO.transform.GetChild(i);
            contentItemsHeight += child.rect.height + 15f;
        }

        RectTransform contentRect = (RectTransform)contentGO.transform;
        contentRect.sizeDelta = new Vector2(0, contentItemsHeight);
    }
}
