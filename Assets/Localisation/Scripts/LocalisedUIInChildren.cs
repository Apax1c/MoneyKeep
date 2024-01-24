/// <summary Created by:>
/// Semionov Alexey; Qplaze; 2020;
/// </summary>

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Скрипт вешается на родительский обьект текста в котором нужно добавить в локализации.
/// Затем нажать кнопку "Добавить дочеркние текста", что бы закешировать дочерние текста
/// Затем нажать "Добавить текста в xml", что бы добавить текст из закешированых текстов в выбраную локализации.
/// 
/// При желании можно вручную перетаскивать текстовые обьекты в поле texts, для кеширования.
/// 
/// Дальше скрипт остаётся на обекте и при старте загружает и выставляет локализированые текста
/// 
/// Работает только на UIText
/// </summary>
public class LocalisedUIInChildren: MonoBehaviour
{
   [SerializeField] private Text[] texts;

    void Awake()
    {
        if(texts.Length<=0)
        {
            Debug.Log("Не выставлен текст!", gameObject);
        }
        foreach(Text text in texts)
        {
            text.text = Localisation.GetString(text.text, text);
        }
    }

#if UNITY_EDITOR
    public void AddChildTexts()
    {
        texts = GetComponentsInChildren<Text>(true);
    }
    public string[] GetTexts()
    {
        if(texts == null)
        {
            return new string[0];
        }

        int textsLeight = texts.Length;
        string[] txts = new string[textsLeight];
        for(int i = 0; i < textsLeight; i++)
        {
            if(texts[i] == null)
            {
                Debug.LogError("Пустой текст! " + i, this);
                return new string[0];
            }
            txts[i] = texts[i].text;
        }
        return txts;
    }
#endif
}
