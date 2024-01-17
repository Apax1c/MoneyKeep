[System.Serializable]
public class TransactionData
{
    public string transactionName;
    public string cardName;
    public string transactionSum;
    public int details;

    public TransactionData(string transactionName, string cardName, string transactionSum, int details)
    {
        this.transactionName = transactionName;
        this.cardName = cardName;
        this.transactionSum = transactionSum;
        this.details = details;
    }
}