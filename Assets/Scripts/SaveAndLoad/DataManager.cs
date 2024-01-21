using System.Collections.Generic;
using System.IO;
using System.Transactions;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private List<TransactionData> transactions = new List<TransactionData>();
    private string filePath;

    [SerializeField] private GameObject NewTransactionHistoryPrefab;
    [SerializeField] private GameObject NewDateItemPrefab;
    [SerializeField] private Transform HistoryContent;

    private string lastItemDate;

    private CategoryDataSource spendingsDataSource;
    private CategoryDataSource profitDataSource;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        spendingsDataSource = (CategoryDataSource)Resources.Load("CategoryDataSource");
        profitDataSource = (CategoryDataSource)Resources.Load("ProfitCategoryDataSource");

        filePath = Path.Combine(Application.persistentDataPath, "transactions.json");

        ClearHistory();
        LoadTransactions();
    }

    public void ClearData()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    private void ClearHistory()
    {
        if (HistoryContent.transform.childCount != 0)
        {
            for (int i = 0; i < HistoryContent.transform.childCount; i++)
            {
                if (HistoryContent.transform.GetChild(i).gameObject != null)
                {
                    Destroy(HistoryContent.transform.GetChild(i).gameObject);
                }
            }
        }
    }

    public void AddOrUpdateTransaction(TransactionData data)
    {
        transactions.Add(data); // Or update existing data
        SaveTransactions();
    }

    private void SaveTransactions()
    {
        string jsonData = JsonUtility.ToJson(new Serialization<TransactionData>(transactions));
        File.WriteAllText(filePath, jsonData);
    }

    private void LoadTransactions()
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            transactions = JsonUtility.FromJson<Serialization<TransactionData>>(jsonData).ToList();
            InstantiateObjects();
        }
    }

    public List<TransactionData> GetHistory()
    {
        if (!File.Exists(filePath))
            return null;
        string jsonData = File.ReadAllText(filePath);
        return JsonUtility.FromJson<Serialization<TransactionData>>(jsonData).ToList();
    } 

    private void InstantiateObjects()
    {
        lastItemDate = transactions[0].date;

        GameObject lastDateItemGO = Instantiate(NewDateItemPrefab, HistoryContent);
        lastDateItemGO.transform.SetAsFirstSibling();

        DateHistoryItem lastDateItem = lastDateItemGO.GetComponent<DateHistoryItem>();
        lastDateItem.SetDateText(lastItemDate);

        foreach (var transaction in transactions)
        {
            if (lastItemDate != transaction.date)
            {
                GameObject newDateItemGO = Instantiate(NewDateItemPrefab, HistoryContent);
                newDateItemGO.transform.SetAsFirstSibling();
                
                DateHistoryItem newDateItem = newDateItemGO.GetComponent<DateHistoryItem>();
                newDateItem.SetDateText(transaction.date);

                lastItemDate = transaction.date;
            }

            // Instantiate your objects here and initialize them with the transaction data
            GameObject newItemGO = Instantiate(NewTransactionHistoryPrefab, HistoryContent);
            newItemGO.transform.SetSiblingIndex(1);
            TransactionHistoryItem newItem = newItemGO.GetComponent<TransactionHistoryItem>();

            if (transaction.transactionSum.Contains("-"))
            {
                newItem.TransactionHistory(
                    transaction.transactionName,
                    transaction.cardName,
                    transaction.transactionSum,
                    spendingsDataSource.lsItems[transaction.details].categoryIcon,
                    spendingsDataSource.lsItems[transaction.details].categoryColor,
                    spendingsDataSource.lsItems[transaction.details].categoryIconColor);
            }
            else
            {
                newItem.TransactionHistory(
                    transaction.transactionName,
                    transaction.cardName,
                    transaction.transactionSum,
                    profitDataSource.lsItems[transaction.details].categoryIcon,
                    profitDataSource.lsItems[transaction.details].categoryColor,
                    profitDataSource.lsItems[transaction.details].categoryIconColor);
            }
        }
    }

    [System.Serializable]
    private class Serialization<T>
    {
        [SerializeField]
        private List<T> target;
        public List<T> ToList() => target;
        public Serialization(List<T> target) => this.target = target;
    }
}
