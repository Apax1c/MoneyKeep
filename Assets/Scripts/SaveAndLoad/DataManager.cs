using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private List<TransactionData> transactions = new List<TransactionData>();
    private string filePath;

    [SerializeField] private GameObject NewTransactionHistoryPrefab;
    [SerializeField] private Transform HistoryContent;

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
        LoadTransactions();
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

    private void InstantiateObjects()
    {
        foreach (var transaction in transactions)
        {
            // Instantiate your objects here and initialize them with the transaction data
            GameObject newItemGO = Instantiate(NewTransactionHistoryPrefab, HistoryContent);
            newItemGO.transform.SetAsFirstSibling();
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
