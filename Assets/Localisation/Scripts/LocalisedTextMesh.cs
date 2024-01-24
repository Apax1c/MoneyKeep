using UnityEngine;

public class LocalisedTextMesh: MonoBehaviour
{
    void Start()
    {
        TextMesh textMesh = GetComponent<TextMesh>();
        if(textMesh != null)
        {
            textMesh.text = Localisation.GetString(textMesh.text,gameObject);
        }
    }
}

	
