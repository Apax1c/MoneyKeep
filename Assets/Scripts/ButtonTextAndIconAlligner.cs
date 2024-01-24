using TMPro;
using UnityEngine;

public class ButtonTextAndIconAlligner : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ConfirmText;
    [SerializeField] private RectTransform ConfirmIcon;

    public float Space;

    private void Start()
    {
        RectTransform rectConfirmText = ConfirmText.GetComponent<RectTransform>();
        rectConfirmText.anchoredPosition = new Vector2(-ConfirmIcon.rect.width / 2 - Space / 2, 0f);
    }

    void Update()
    {
        RectTransform rectConfirmText = ConfirmText.GetComponent<RectTransform>();

        ConfirmIcon.localPosition = new Vector3((ConfirmText.preferredWidth / 2) + rectConfirmText.anchoredPosition.x + Space + ConfirmIcon.rect.width / 2, 0, 0);
    }
}