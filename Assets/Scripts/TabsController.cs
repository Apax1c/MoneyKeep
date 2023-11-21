using UnityEngine;

public class TabsController : MonoBehaviour
{
    [SerializeField] private Animator historyAnimator;
    [SerializeField] private Animator categoriesAnimator;
    [SerializeField] private Animator goalsAnimator;

    private const string IS_TOGGLED = "isToggled";

    void Start()
    {
        historyAnimator.SetBool(IS_TOGGLED, true);
        categoriesAnimator.SetBool(IS_TOGGLED, false);
        goalsAnimator.SetBool(IS_TOGGLED, false);
    }

    public void ToggleTab(Animator choosedAnimator)
    {
        historyAnimator.SetBool(IS_TOGGLED, false);
        categoriesAnimator.SetBool(IS_TOGGLED, false);
        goalsAnimator.SetBool(IS_TOGGLED, false);

        choosedAnimator.SetBool(IS_TOGGLED, true);
    }
}
