using System;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentDateText;

    [SerializeField] private GameObject NewCardMenu;

    // Start is called before the first frame update
    void Start()
    {
        currentDateText.text = (char.ToUpper(DateTime.Now.ToString("MMMM, yyyy")[0]) + DateTime.Now.ToString("MMMM, yyyy").Substring(1));
    }
}