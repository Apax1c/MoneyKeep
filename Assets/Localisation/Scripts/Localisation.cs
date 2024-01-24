using UnityEngine;
using System;
using System.Collections.Generic;
using System.Xml;

#if UNITY_EDITOR
using UnityEditor;
#endif


public static class Localisation
{
	public static SystemLanguage CurrentLanguage = SystemLanguage.English;

	private static Dictionary<string, string> stringsDict;
	private static XmlDocument languageXMLFile;
	private static TextAsset languageAsset;

	private static bool isLanguageLoaded = false;

	public static void DetectLanguage()
	{
		CurrentLanguage = Application.systemLanguage;


#if UNITY_EDITOR
		if(EditorPrefs.HasKey("TestLanguage"))
		{
			CurrentLanguage = (SystemLanguage)Enum.Parse(typeof(SystemLanguage), EditorPrefs.GetString("TestLanguage"));
		}
#endif

		Debug.Log("LoadLanguage: " + CurrentLanguage);
	}

	public static void LoadLanguage()
	{
		languageXMLFile = new XmlDocument();
		stringsDict = new Dictionary<string, string>();


		languageAsset = Resources.Load<TextAsset>("Localisation/" + CurrentLanguage);

		if(languageAsset == null) //if no localisation as system language, load english
		{
			Debug.LogErrorFormat("Файл с языком {0} не найден, загружен стандартный: English", CurrentLanguage);
			languageAsset = Resources.Load<TextAsset>("Localisation/English");
		}

		languageXMLFile.LoadXml(languageAsset.text);

		XmlElement root = languageXMLFile.DocumentElement;
		XmlNodeList nodes = root.SelectNodes("//string"); //ignore all nodes but <string> 

		foreach(XmlNode node in nodes)
		{
			string key = node.Attributes["name"].Value;
			if(!stringsDict.ContainsKey(key))
			{
				stringsDict.Add(key, node.InnerText);
			}
			else
			{
				Debug.LogErrorFormat("Alias '{0}' уже существует в файле {1}", key, CurrentLanguage);
				//throw new ArgumentException("An element with'"+key+"' key already exists in the dictionary");\
			}
		}
		isLanguageLoaded = true;
	}

	public static string GetString(string searchString, UnityEngine.Object context = null)
	{
		if(!isLanguageLoaded)
		{
			DetectLanguage();
			LoadLanguage();
		}

		if(stringsDict.ContainsKey(searchString))
		{
			return stringsDict[searchString];
		}
		else
		{
			Debug.LogError("Unknown string: '" + searchString + "'", context);
			return "^" + searchString;
		}

	}

	public static string GetStringWithValues(string searchString, params string[] values)
	{
		return string.Format(Localisation.GetString(searchString), values);
	}

}