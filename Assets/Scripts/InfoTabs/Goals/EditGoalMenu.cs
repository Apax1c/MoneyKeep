using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditGoalMenu : MonoBehaviour
{
    public static EditGoalMenu instance;

    private Animator editGoalMenuAnimator;
    [SerializeField] private Animator currencyAnimator;
    private const string IS_MENU_TOGGLED = "isMenuToggled";
    private const string CURRENCY_CHOOSE_ID = "currencyChooseId";
    private bool isMenuToggled = false;

    private int goalId;

    [SerializeField] private TMP_InputField goalName;
    [SerializeField] private TMP_InputField startSumm;
    [SerializeField] private TMP_InputField goalSumm;

    [SerializeField] private TextMeshProUGUI[] currencies;

    [SerializeField] private Button confirmButton;

    private GoalItem goalItemScript;

    private string newGoalName;
    private float newGoalStartSumm;
    private float newGoalSumm;
    private string currency;

    private bool isNameSetted = true;
    private bool isStartSummParseable = true;
    private bool isSummParseable = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        editGoalMenuAnimator = GetComponent<Animator>();

        currencyAnimator.SetInteger(CURRENCY_CHOOSE_ID, 0);
    }

    public void OpenMenu(int id, GoalItem goalScript)
    {
        isMenuToggled = true;
        editGoalMenuAnimator.SetBool(IS_MENU_TOGGLED, isMenuToggled);

        goalItemScript = goalScript;
        goalId = id;
        SetData();
    }

    public void CloseMenu()
    {
        isMenuToggled = false;
        editGoalMenuAnimator.SetBool(IS_MENU_TOGGLED, isMenuToggled);
    }

    public void SetGoalName(string newName)
    {
        newGoalName = newName;

        if (newGoalName != string.Empty)
            isNameSetted = true;
        else
            isNameSetted = false;

        SetButtonInteractable();
    }

    public void SetGoalStartSumm(string newStartSumm)
    {
        if (float.TryParse(newStartSumm.Replace(".", ","), out newGoalStartSumm))
            isStartSummParseable = true;
        else if (newStartSumm == string.Empty)
        {
            isStartSummParseable = true;
            newGoalStartSumm = 0.00f;
        }
        else
            isStartSummParseable = false;

        SetButtonInteractable();
    }

    public void SetGoalSumm(string newSumm)
    {
        if (float.TryParse(newSumm.Replace(".", ","), out newGoalSumm))
            isSummParseable = true;
        else
            isSummParseable = false;

        SetButtonInteractable();
    }

    private void SetButtonInteractable()
    {
        if (isStartSummParseable && isSummParseable && isNameSetted)
        {
            confirmButton.interactable = true;
        }
        else
        {
            confirmButton.interactable = false;
        }
    }

    public void SetCurrency(string newCurrency, int currecnyId = 0)
    {
        currency = newCurrency;
        for (int i = 0; i < currencies.Length; i++)
        {
            currencies[i].text = currency;
        }

        currencyAnimator.SetInteger(CURRENCY_CHOOSE_ID, currecnyId);
    }
      
    private void SetData()
    {
        List<GoalData> dataList = DataManager.Instance.GetGoals();

        goalName.text = dataList[goalId].goalName;
        startSumm.text = dataList[goalId].currentSum.ToString();
        goalSumm.text = dataList[goalId].goalSum.ToString();
    }

    public void DeleteGoal()
    {
        DataManager.Instance.DeleteGoalByIndex(goalId);
        Destroy(goalItemScript.gameObject);

        CloseMenu();
    }

    public void OnConfirm()
    {
        GoalData goal = new GoalData(newGoalName, newGoalStartSumm, newGoalSumm, currency);
        goalItemScript.SetGoal(goal, goalId);

        DataManager.Instance.UpdateGoalByIndex(goalId, goal);

        CloseMenu();
    }
}
