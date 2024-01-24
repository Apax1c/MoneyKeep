using UnityEngine;
using UnityEngine.UI;
using System;


public class LocalisedUIText : MonoBehaviour {

	private void Start ()
	{
		Text text = GetComponent<Text>();
		if (text != null)
		{
			text.text = Localisation.GetString(text.text, gameObject);
		}
	}
}

