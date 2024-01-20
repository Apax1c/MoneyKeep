using TMPro;
using Unity.VectorGraphics;
using UnityEngine;

public class TransactionHistoryItem : MonoBehaviour
{
    public TextMeshProUGUI transactionNameText;
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI transactionSummText;
    public SVGImage categoryBackground;
    public SVGImage categoryIcon;

    public void TransactionHistory(string transactionName, string cardName, string transactionSumm, Sprite categoryIconSprite, Color categoryBackgroundColor, Color categoryIconColor)
    {
        transactionNameText.text = transactionName;
        cardNameText.text = cardName;
        transactionSummText.text = transactionSumm;
        categoryIcon.sprite = categoryIconSprite;
        categoryBackground.color = categoryBackgroundColor;
        categoryIcon.color = categoryIconColor;
    }
}
