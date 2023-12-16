using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CategoryData", menuName = "CategoryData")]
public class CategoryDataSO : ScriptableObject
{
    public List<Color> colors;
    public List<Color> iconColor;
    public List<Sprite> icons;
}