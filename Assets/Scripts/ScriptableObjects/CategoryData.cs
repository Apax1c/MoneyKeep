using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class CategoryData
{
    public int id;
    public Color categoryColor;
    public Color categoryIconColor;
    public Sprite categoryIcon;
    public string categoryName;
}

public abstract class DataTable<T> : ScriptableObject
{
    public T[] lsItems;

    /// <summary>
    /// Get config by id or level
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public abstract T GetConfigData(int id);
}