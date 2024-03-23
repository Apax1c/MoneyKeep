using UnityEngine;

public class GoalsScrollView : MonoBehaviour
{
    [SerializeField] private GameObject goalItemPrefab;

    private float contentItemsHeight = 0;
    public float contentItemsSpace;
    [SerializeField] private Transform contentGO;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateContentSize();
    }

    public void UpdateContentSize()
    {
        contentItemsHeight = 0;

        for (int i = 0; i < contentGO.childCount; i++)
        {
            RectTransform child = (RectTransform)contentGO.GetChild(i);
            contentItemsHeight += child.rect.height;
            contentItemsHeight += contentItemsSpace;
        }

        RectTransform contentRect = (RectTransform)contentGO;
        contentRect.sizeDelta = new Vector2(0, contentItemsHeight);
    }
}