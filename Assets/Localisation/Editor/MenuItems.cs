using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class MenuItems
{
    [MenuItem("Localisation/Reset to system language", false, -10)]
    private static void ClearTestLanguage()
    {
        EditorPrefs.DeleteKey("TestLanguage");
    }
	[MenuItem("Localisation/Unknown",false,1)]
	private static void SetLanguageUnknown()
	{
		EditorPrefs.SetString("TestLanguage","Unknown");
    }
	[MenuItem("Localisation/Russian",false,12)]
	private static void SetLanguageRU()
	{
        EditorPrefs.SetString("TestLanguage", "Russian");
    }
	[MenuItem("Localisation/Ukrainian",false,12)]
	private static void SetLanguageUA()
	{
		EditorPrefs.SetString("TestLanguage","Ukrainian");
    }
	[MenuItem("Localisation/English",false,23)]
	private static void SetLanguageEN()
	{
        EditorPrefs.SetString("TestLanguage", "English");
    }
	[MenuItem("Localisation/Italian",false,23)]
	private static void SetLanguageIT()
	{
		EditorPrefs.SetString("TestLanguage","Italian");
    }
	[MenuItem("Localisation/Spanish",false,23)]
	private static void SetLanguageSP()
	{
		EditorPrefs.SetString("TestLanguage","Spanish");
    }
	[MenuItem("Localisation/French",false,23)]
	private static void SetLanguageFR()
	{
		EditorPrefs.SetString("TestLanguage","French");
    }
	[MenuItem("Localisation/German",false,23)]
	private static void SetLanguageDE()
	{
		EditorPrefs.SetString("TestLanguage","German");
    }
	[MenuItem("Localisation/Polish",false,23)]
	private static void SetLanguagePL()
	{
		EditorPrefs.SetString("TestLanguage","Polish");
    }
	[MenuItem("Localisation/Chinese",false,34)]
	private static void SetLanguageCN()
	{
		EditorPrefs.SetString("TestLanguage","Chinese");
    }
    [MenuItem("Localisation/Chinese Simplified", false, 34)]
    private static void SetLanguageCNs()
    {
        EditorPrefs.SetString("TestLanguage", "ChineseSimplified");
    }
    [MenuItem("Localisation/Chinese Traditional", false, 34)]
    private static void SetLanguageCNt()
    {
        EditorPrefs.SetString("TestLanguage", "ChineseTraditional");
    }
	[MenuItem("Localisation/Arabic")]
	private static void SetLanguageAR()
	{
		EditorPrefs.SetString("TestLanguage","Arabic");
    }
	[MenuItem("Localisation/Portuguese")]
	private static void SetLanguagePR()
	{
		EditorPrefs.SetString("TestLanguage","Portuguese");
    }
	[MenuItem("Localisation/Turkish")]
	private static void SetLanguageTU()
	{
		EditorPrefs.SetString("TestLanguage","Turkish");
    }

	//Удаление PlayerPrefs
	[MenuItem("Tools/Clear PlayerPrefs")]
	private static void NewMenuOption()
	{
		PlayerPrefs.DeleteAll();
	}
}

