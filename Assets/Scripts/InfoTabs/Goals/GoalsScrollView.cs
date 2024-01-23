using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoalsScrollView : MonoBehaviour
{
    [Header("Confirm Button")]
    [SerializeField] private TextMeshProUGUI ConfirmText;
    [SerializeField] private Transform ConfirmIcon;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ConfirmIcon.localPosition = new Vector3((ConfirmText.preferredWidth / 2) - 24f + 10f + 24f / 2, 0, 0);
    }
}
