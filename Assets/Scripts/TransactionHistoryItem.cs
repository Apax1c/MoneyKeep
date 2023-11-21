using TMPro;
using Unity.VectorGraphics;
using UnityEngine;

public class TransactionHistoryItem : MonoBehaviour
{
    public TextMeshProUGUI transactionNameText;
    public TextMeshProUGUI transactionTimeText;
    public TextMeshProUGUI transactionSummText;
    public SVGImage categoryIcon;

    public void TransactionHistory(string transactionName, string transactionTime, string transactionSumm, Sprite categoryIconSprite)
    {
        transactionNameText.text = transactionName;
        transactionTimeText.text = transactionTime;
        transactionSummText.text = transactionSumm;
        categoryIcon.sprite = categoryIconSprite;
    }
}
