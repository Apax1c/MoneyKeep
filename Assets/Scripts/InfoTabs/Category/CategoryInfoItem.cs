using TMPro;
using Unity.VectorGraphics;
using UnityEngine;

public class CategoryInfoItem : MonoBehaviour
{
    [SerializeField] private SVGImage backgroundImage;
    [SerializeField] private SVGImage iconImage;

    [SerializeField] private TextMeshProUGUI differenceSumText;
    [SerializeField] private TextMeshProUGUI categoryNameText;

    private int categoryId;

    public void SetCategory(CategoryData data)
    {
        categoryId = data.id;

        categoryNameText.text = data.categoryName;
        backgroundImage.color = data.categoryColor;
        iconImage.sprite = data.categoryIcon;
        iconImage.color = data.categoryIconColor;
    }
}
