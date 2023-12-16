using TMPro;
using Unity.VectorGraphics;
using UnityEngine;

public class TransactionHistoryItem : MonoBehaviour
{
    public TextMeshProUGUI transactionNameText;
    public TextMeshProUGUI transactionTimeText;
    public TextMeshProUGUI transactionSummText;
    public SVGImage categoryBackground;
    public SVGImage categoryIcon;

    public void TransactionHistory(string transactionName, string transactionTime, string transactionSumm, Sprite categoryIconSprite, Color categoryBackgroundColor, Color categoryIconColor)
    {
        transactionNameText.text = transactionName;
        transactionTimeText.text = transactionTime;
        transactionSummText.text = transactionSumm;
        categoryIcon.sprite = categoryIconSprite;
        categoryBackground.color = categoryBackgroundColor;
        categoryIcon.color = categoryIconColor;
    }
}
