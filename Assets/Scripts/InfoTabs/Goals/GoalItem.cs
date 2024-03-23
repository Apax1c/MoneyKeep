using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class GoalItem : MonoBehaviour
{
    [SerializeField] private SVGImage backgroundImage;
    [SerializeField] private Image secondImage;

    [SerializeField] private TextMeshProUGUI goalNameText;
    [SerializeField] private TextMeshProUGUI goalCurrentSummText;
    [SerializeField] private TextMeshProUGUI goalSummText;

    [SerializeField] private Slider goalProgressBar;

    private int goalId;

    public void SetGoal(GoalData data, int id)
    {
        goalId = id;

        goalNameText.text = data.goalName;
        goalCurrentSummText.text = TextColors.ApplyColorToText(new Color(0.6588235f, 0.8f, 0.5176471f), data.currency) +
            TextColors.ApplyColorToText(new Color(0.1058824f, 0.1058824f, 0.1058824f), data.currentSum.ToString().Replace(",", "."));
        goalSummText.text = TextColors.ApplyColorToText(new Color(0.6588235f, 0.8f, 0.5176471f), data.currency) +
            TextColors.ApplyColorToText(new Color(0.1058824f, 0.1058824f, 0.1058824f), data.goalSum.ToString().Replace(",", "."));

        UpdateProgressBar(data.currentSum, data.goalSum);
    }

    private void UpdateProgressBar(float current, float goal)
    {
        goalProgressBar.value = current/goal;
    }

    public void OnItemPressed()
    {
        EditGoalMenu.instance.OpenMenu(goalId, this);
    }
}