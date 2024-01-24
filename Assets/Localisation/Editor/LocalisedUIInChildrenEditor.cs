/// <summary Created by:>
/// Semionov Alexey; Qplaze; 2020;
/// </summary>

using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LocalisedUIInChildren))]
public class LocalisedUIInChildrenEditor : Editor
{
    bool isRussian = false;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LocalisedUIInChildren localisedUI = (LocalisedUIInChildren)target;
        
        if(GUILayout.Button("Добавить дочеркние текста"))
        {
            localisedUI.AddChildTexts();
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Добавление в: " + (isRussian ? SystemLanguage.Russian : SystemLanguage.English));//Изначально доступны только эти 2, да и врядли когда то понадобится использовать другие языки
		if(GUILayout.Button("Поменять язык"))
        {
            isRussian = !isRussian;
        }

        EditorGUILayout.EndHorizontal();


		string[] texts = localisedUI.GetTexts();
		if(Event.current.type != EventType.DragPerform && texts.Length > 0)
		{
			if(GUILayout.Button("Добавить текста в xml"))
			{
				AddAliasAndLocalisationToXML(texts, isRussian ? SystemLanguage.Russian : SystemLanguage.English);
			}
		}
	}
	private void AddAliasAndLocalisationToXML(string[] sourceTexts, SystemLanguage lang)
	{
		//ВАЖНО!! Возможна ошибка когда на один и тот же алиас имеются разные текста.

		//Загружаем имеющийся документ
		XmlDocument xmlDoc = new XmlDocument();
		TextAsset langAsset = Resources.Load<TextAsset>("Localisation/" + lang);
		xmlDoc.LoadXml(langAsset.text);

		//Добавляем все имеющиеся в документе алиасы в HashSet. Нужно что бы не добавлять имеющиеся алиасы.
		XmlElement root = xmlDoc.DocumentElement;
		XmlNodeList nodes = root.SelectNodes("//string"); //ignore all nodes but <string> 
		HashSet<string> dict = new HashSet<string>();

		foreach(XmlNode node in nodes)
		{
			string key = node.Attributes["name"].Value;
			if(!dict.Contains(key))
			{
				dict.Add(key);
			}

		}

		XmlNode rootNode = xmlDoc.GetElementsByTagName("document")[0];

		//int i = 0;//Нужно для более краткого наименования
		foreach(var txt in sourceTexts)//Добавляем новые алиасы и текст в загруженый документ
		{
			if(!dict.Contains(txt))//Проверяем что бы не доавлялись алиасы которые уже есть
			{
				XmlNode node = xmlDoc.CreateElement("string");
				XmlAttribute attribute = xmlDoc.CreateAttribute("name");
				//attribute.Value = "ID"+i;
				attribute.Value = txt;
				node.Attributes.Append(attribute);
				var value = txt;
				node.InnerText = value;
				rootNode.AppendChild(node);
				//i++;

				dict.Add(txt);//Добавляем новый алиас в словарь, что бы избижать повторений
							  //ВАЖНО!! Возможна ошибка когда на один и тот же алиас имеются разные текста.
			}
		}

		//Перезаписываем докупент, сохраняем и обновляем ассеты.
		string path = AssetDatabase.GetAssetPath(langAsset);
		xmlDoc.Save(path);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		Debug.Log("Сохранено в " + path);

	}
}
