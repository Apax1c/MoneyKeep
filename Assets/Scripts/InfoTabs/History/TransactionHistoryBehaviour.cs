using UnityEngine;

public class TransactionHistoryBehaviour : MonoBehaviour
{
    private float contentItemsHeight = 0;
    [SerializeField] private Transform content;

    // Start is called before the first frame update
    void Update()
    {
        UpdateContentSize();
    }

    public void UpdateContentSize()
    {
        contentItemsHeight = 0;

        for (int i = 0; i < content.childCount; i++)
        {
            RectTransform child = (RectTransform)content.GetChild(i);
            contentItemsHeight += child.rect.height;
        }

        RectTransform contentRect = (RectTransform)content;
        contentRect.sizeDelta = new Vector2(0, contentItemsHeight);
    }
}
