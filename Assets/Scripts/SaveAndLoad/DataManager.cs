using System.Collections.Generic;
using System.IO;
using System.Transactions;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private List<TransactionData> transactions = new List<TransactionData>();
    private List<GoalData> goals = new List<GoalData>();
    private string transactionFilePath;
    private string goalFilePath;

    [SerializeField] private GameObject NewTransactionHistoryPrefab;
    [SerializeField] private GameObject NewDateItemPrefab;
    [SerializeField] private GameObject NewGoalItemPrefab;

    [SerializeField] private Transform HistoryContent;
    [SerializeField] private Transform GoalContent;

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

        transactionFilePath = Path.Combine(Application.persistentDataPath, "transactions.json");
        goalFilePath = Path.Combine(Application.persistentDataPath, "goal.json");

        ClearHistory();
        LoadTransactions();
        LoadGoals();
    }

    public void ClearData()
    {
        if (File.Exists(transactionFilePath))
        {
            File.Delete(transactionFilePath);
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
        File.WriteAllText(transactionFilePath, jsonData);
    }

    private void LoadTransactions()
    {
        if (File.Exists(transactionFilePath))
        {
            string jsonData = File.ReadAllText(transactionFilePath);
            transactions = JsonUtility.FromJson<Serialization<TransactionData>>(jsonData).ToList();
            InstantiateTransactions();
        }
    }

    public List<TransactionData> GetHistory()
    {
        if (!File.Exists(transactionFilePath))
            return null;
        string jsonData = File.ReadAllText(transactionFilePath);
        return JsonUtility.FromJson<Serialization<TransactionData>>(jsonData).ToList();
    }

    public void AddOrUpdateGoals(GoalData data)
    {
        goals.Add(data); // Or update existing data
        SaveGoals();
    }

    private void SaveGoals()
    {
        string jsonData = JsonUtility.ToJson(new Serialization<GoalData>(goals));
        File.WriteAllText(goalFilePath, jsonData);
    }

    private void LoadGoals()
    {
        if (File.Exists(goalFilePath))
        {
            string jsonData = File.ReadAllText(goalFilePath);
            goals = JsonUtility.FromJson<Serialization<GoalData>>(jsonData).ToList();
            InstantiateGoals();
        }
    }

    public List<GoalData> GetGoals()
    {
        if (!File.Exists(goalFilePath))
            return null;
        string jsonData = File.ReadAllText(goalFilePath);
        return JsonUtility.FromJson<Serialization<GoalData>>(jsonData).ToList();
    }

    public void UpdateGoalByIndex(int id, GoalData goalData)
    {
        goals[id] = goalData;
        SaveGoals();
    }

    public void DeleteGoalByIndex(int id)
    {
        goals.RemoveAt(id);
        SaveGoals();
    }

    private void InstantiateTransactions()
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

    private void InstantiateGoals()
    {
        int i = 0;

        foreach(var goal in goals)
        {
            GameObject newGoalItemGO = Instantiate(NewGoalItemPrefab, GoalContent);
            newGoalItemGO.transform.SetAsFirstSibling();

            GoalItem goalItemScript = newGoalItemGO.GetComponent<GoalItem>();
            goalItemScript.SetGoal(goal, i);
            newGoalItemGO.transform.SetAsFirstSibling();

            i++;
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
