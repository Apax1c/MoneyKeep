using UnityEngine;
using TMPro;

public class LocalisedTextMeshProUGUI : MonoBehaviour
{
    private TextMeshProUGUI _text;

    [Header("Key")]
    [SerializeField] private string _key;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();

        if (_text != null)
        {
            _text.text = Localisation.GetString(_key, gameObject);
        }
    }
}
