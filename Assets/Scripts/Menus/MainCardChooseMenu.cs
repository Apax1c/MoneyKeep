using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCardChooseMenu : MonoBehaviour
{
    public static MainCardChooseMenu instance;

    private Animator menuAnimator;

    [SerializeField] private CardBehaviour cardBehaviour;

    // GameObjects
    [SerializeField] private GameObject ChooseCardItemPrefab;
    [SerializeField] private GameObject contentGO;

    public int cardId { get; private set; }
    private float contentItemsHeight = 0;

    private const string IS_MENU_TOGGLED = "isMenuToggled";
    private bool isMenuToggled = false;

    public event EventHandler OnMainCardChoosed;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        menuAnimator = GetComponent<Animator>();

        cardId = cardBehaviour.CardId;
        ClearList();
    }

    public void SetCardId(int newCardId)
    {
        cardId = newCardId;

        OnMainCardChoosed?.Invoke(this, EventArgs.Empty);
    }

    private void ClearList()
    {
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
    }

    private void LoadList()
    {
        for (int i = 0; i < Card.CardList.Count; i++)
        {
            GameObject newChooseCardItem = Instantiate(ChooseCardItemPrefab, contentGO.transform);
            newChooseCardItem.GetComponent<MainCardChooseItemDisplay>().SetCardId(i);
        }

        UpdateContentSize();
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

    public void ToggleMenu()
    {
        isMenuToggled = !isMenuToggled;
        menuAnimator.SetBool(IS_MENU_TOGGLED, isMenuToggled);

        ClearList();
        LoadList();
    }

    public void CloseMenu()
    {
        isMenuToggled = false;

        menuAnimator.SetBool(IS_MENU_TOGGLED, isMenuToggled);
    }
}
