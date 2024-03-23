[System.Serializable]

public class GoalData
{
    public string goalName;
    public float currentSum;
    public float goalSum;
    public string currency;

    public GoalData(string goalName, float currentSum, float goalSum, string currency)
    {
        this.goalName = goalName;
        this.currentSum = currentSum;
        this.goalSum = goalSum;
        this.currency = currency;
    }
}