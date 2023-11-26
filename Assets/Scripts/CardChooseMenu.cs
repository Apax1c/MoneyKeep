using System;
using UnityEngine;

public class CardChooseMenu : MonoBehaviour
{
    public static CardChooseMenu instance;

    [SerializeField] private GameObject ChooseCardItemPrefab;
    [SerializeField] private GameObject contentGO;


    private Animator chooseCardAnimator;

    private const string IS_MENU_TOGGLED = "isMenuToggled";

    // Variables
    private float contentItemsHeight = 0;
    private bool isMenuToggled = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
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
                    Debug.Log("Test");
                    Destroy(contentGO.transform.GetChild(i).gameObject);
                }
            }
        }

        for (int i = 0; i < Card.CardList.Count; i++)
        {
            GameObject newChooseCardItem = Instantiate(ChooseCardItemPrefab, contentGO.transform);
            newChooseCardItem.GetComponent<ChooseCardItemDisplay>().SetCardId(i);
        }

        UpdateContentSize();
    }

    public void OpenMenu()
    {
        isMenuToggled = true;
        chooseCardAnimator.SetBool(IS_MENU_TOGGLED, true);
        LoadChooseCardItems();
    }

    public void CloseMenu()
    {
        isMenuToggled = false;
        chooseCardAnimator.SetBool(IS_MENU_TOGGLED, false);
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
