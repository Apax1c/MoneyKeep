using System;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class NewGoalMenu : MonoBehaviour
{
    public static NewGoalMenu instance;

    private Animator newGoalMenuAnimator;
    [SerializeField] private Animator currencyAnimator;
    private const string IS_MENU_TOGGLED = "isMenuToggled";
    private const string CURRENCY_CHOOSE_ID = "currencyChooseId";
    private bool isMenuToggled = false;

    [SerializeField] private GameObject newGoalPrefabGO;
    [SerializeField] private Transform newGoalParent;

    [SerializeField] private TextMeshProUGUI[] currencies;

    [SerializeField] private Button confirmButton;

    private string newGoalName;
    private float newGoalStartSumm;
    private float newGoalSumm;
    private string currency;

    private bool isNameSetted = false;
    private bool isStartSummParseable = true;
    private bool isSummParseable = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        newGoalMenuAnimator = GetComponent<Animator>();

        currencyAnimator.SetInteger(CURRENCY_CHOOSE_ID, 0);
        SetCurrency("$");
        SetButtonInteractable();
    }

    public void OpenMenu()
    {
        isMenuToggled = true;
        newGoalMenuAnimator.SetBool(IS_MENU_TOGGLED, isMenuToggled);
    }

    public void CloseMenu()
    {
        isMenuToggled = false;
        newGoalMenuAnimator.SetBool(IS_MENU_TOGGLED, isMenuToggled);
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

    public void Confirm()
    {
        GameObject newGoalGO = Instantiate(newGoalPrefabGO, newGoalParent);
        newGoalGO.transform.SetAsFirstSibling();

        GoalData goal = new GoalData(newGoalName, newGoalStartSumm, newGoalSumm, currency);
        GoalItem newGoalScript = newGoalGO.GetComponent<GoalItem>();
        newGoalScript.SetGoal(goal, DataManager.Instance.GetGoals().Count);

        DataManager.Instance.AddOrUpdateGoals(goal);

        CloseMenu();
    }
}
