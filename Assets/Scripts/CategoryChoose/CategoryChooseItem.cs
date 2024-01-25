using TMPro;
using Unity.VectorGraphics;
using UnityEngine;

public class CategoryChooseItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI categoryNameText;

    [SerializeField] private GameObject categoryFrame;

    [SerializeField] private SVGImage backgroundImage;
    [SerializeField] private SVGImage iconImage;

    private CategoryChooseMenu categoryChooseMenuScript;

    private int categoryId;

    private void Start()
    {
        categoryChooseMenuScript = CategoryChooseMenu.instance;
    }

    private void Update()
    {
        if (categoryChooseMenuScript.choosedCategoryId == categoryId)
        {
            categoryFrame.SetActive(true);
        }
        else
        {
            categoryFrame.SetActive(false);
        }
    }

    public void SetCategory(CategoryData data)
    {
        categoryId = data.id;

        categoryNameText.text = Localisation.GetString(data.categoryName, this);
        backgroundImage.color = data.categoryColor;
        iconImage.sprite = data.categoryIcon;
        iconImage.color = data.categoryIconColor;
        categoryFrame.GetComponent<SVGImage>().color = data.categoryIconColor;
    }

    public void ChooseCategoryId()
    {
        categoryChooseMenuScript.SetCategoryId(categoryId);
    }
}