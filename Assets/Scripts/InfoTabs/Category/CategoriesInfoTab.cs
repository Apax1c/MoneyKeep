using System;
using UnityEngine;

public class CategoriesInfoTab : MonoBehaviour
{
    [SerializeField] private GameObject categoryInfoItemPrefab;

    private float contentItemsHeight = 0;
    [SerializeField] private Transform contentGO;

    void Start()
    {
        gameObject.SetActive(false);
        LoadChooseCardItems();
    }

    private void LoadChooseCardItems()
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

        CategoryDataSource categoryDataSource = (CategoryDataSource)Resources.Load("CategoryDataSource");

        for (int i = 0; i < categoryDataSource.lsItems.Length; i++)
        {
            GameObject newCategoryCardItem = Instantiate(categoryInfoItemPrefab, contentGO.transform);
            newCategoryCardItem.GetComponent<CategoryInfoItem>().SetCategory(categoryDataSource.lsItems[i]);
        }

        UpdateContentSize();
    }

    public void UpdateContentSize()
    {
        contentItemsHeight = 0;

        for (int i = 0; i < contentGO.childCount; i++)
        {
            RectTransform child = (RectTransform)contentGO.GetChild(i);
            contentItemsHeight += child.rect.height;
        }

        RectTransform contentRect = (RectTransform)contentGO;
        contentRect.sizeDelta = new Vector2(0, contentItemsHeight);
    }
}
