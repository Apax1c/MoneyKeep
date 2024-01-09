using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;

public class ProfitCategoryChooseMenu : MonoBehaviour
{
    // Singleton
    public static ProfitCategoryChooseMenu instance;

    [Header("GameObjects")]
    [SerializeField] private GameObject categoryItemPrefab;
    [SerializeField] private GameObject contentGO;

    [Header("Input Field")]
    [SerializeField] private SVGImage inputFieldFrame;
    [SerializeField] private SVGImage inputFieldIcon;
    [SerializeField] private TMP_InputField categoryInputField;

    private ProfitMenu spendingsMenuScript;

    private Animator chooseCardAnimator;

    private const string IS_MENU_TOGGLED = "isMenuToggled";

    // Variables
    private float contentItemsHeight = 0;
    private bool isMenuToggled = false;
    public int choosedCategoryId { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        spendingsMenuScript = ProfitMenu.instance;
        choosedCategoryId = 0;

        LoadCategoryItems();

        chooseCardAnimator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (categoryInputField.text != string.Empty)
        {
            inputFieldIcon.color = new Color(0.6509804f, 0.9607843f, 0.8313726f, 1f);
            inputFieldFrame.color = new Color(0.6509804f, 0.9607843f, 0.8313726f, 1f);
        }
        else
        {
            inputFieldIcon.color = new Color(0.8941177f, 0.8941177f, 0.8941177f, 1);
            inputFieldFrame.color = new Color(0.8941177f, 0.8941177f, 0.8941177f, 1);
        }
    }

    private void LoadCategoryItems()
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

        CategoryDataSource categoryDataSource = (CategoryDataSource)Resources.Load("ProfitCategoryDataSource");

        for (int i = 0; i < categoryDataSource.lsItems.Length; i++)
        {
            GameObject newCategoryCardItem = Instantiate(categoryItemPrefab, contentGO.transform);
            newCategoryCardItem.GetComponent<ProfitCategoryChooseItem>().SetCategory(categoryDataSource.lsItems[i]);
        }

        UpdateContentSize();
    }

    public void OpenMenu()
    {
        isMenuToggled = true;
        categoryInputField.text = string.Empty;
        chooseCardAnimator.SetBool(IS_MENU_TOGGLED, isMenuToggled);
    }

    public void CloseMenu()
    {
        isMenuToggled = false;
        chooseCardAnimator.SetBool(IS_MENU_TOGGLED, isMenuToggled);
    }

    public void ConfrimCategory()
    {
        if (categoryInputField.text != string.Empty)
        {
            spendingsMenuScript.UpdateCategoryId(choosedCategoryId, categoryInputField.text);
        }
        else
        {
            spendingsMenuScript.UpdateCategoryId(choosedCategoryId);
        }

        CloseMenu();
    }

    private void UpdateContentSize()
    {
        contentItemsHeight = 0;

        for (int i = 0; i < Card.CardList.Count; i++)
        {
            RectTransform child = (RectTransform)contentGO.transform.GetChild(i);
            contentItemsHeight += child.rect.height + 60f;
        }

        RectTransform contentRect = (RectTransform)contentGO.transform;
        contentRect.sizeDelta = new Vector2(0, contentItemsHeight);
    }

    public void SetCategoryId(int newCategoryId)
    {
        choosedCategoryId = newCategoryId;
    }
}
