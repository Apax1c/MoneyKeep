[System.Serializable]
public class TransactionData
{
    public string transactionName;
    public string cardName;
    public string transactionSum;
    public int details;
    public string date;

    public string currency;
    public string currencyCode;

    public TransactionData(string transactionName, string cardName, string transactionSum, int details, string date, string currency, string currencyCode)
    {
        this.transactionName = transactionName;
        this.cardName = cardName;
        this.transactionSum = transactionSum;
        this.details = details;
        this.date = date;
        this.currency = currency;
        this.currencyCode = currencyCode;
    }
}