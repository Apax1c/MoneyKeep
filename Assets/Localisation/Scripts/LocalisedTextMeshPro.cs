using UnityEngine;
using TMPro;

public class LocalisedTextMeshPro : MonoBehaviour
{
	void Start()
	{
		// For Meshes
		TextMeshPro textMesh = GetComponent<TextMeshPro>();
		if (textMesh != null)
		{
			textMesh.text = Localisation.GetString(textMesh.text, gameObject);
		}
	}
}
