using System.Collections.Generic;
using UnityEngine;

public static class TextColors
{
    public enum DefaultColorsEnum
    {
        Green,
        White,
        Red,
        Black
    }

    public static Dictionary<DefaultColorsEnum, string> ColorsToStringList { get; private set; } = new Dictionary<DefaultColorsEnum, string>()
    {
        { DefaultColorsEnum.Green, "<color=#5EDEA9>" },
        { DefaultColorsEnum.White, "<color=#F6F6F6>" },
        { DefaultColorsEnum.Red, "<color=#ED808D>" },
        { DefaultColorsEnum.Black, "<color=#1B1B1B>" },
    };

    public static string ApplyColorToText(DefaultColorsEnum colors, string text)
    {
        return ColorsToStringList[colors] + text;
    }

    public static string ApplyColorToText(Color colors, string text)
    {
        string hexColor = ColorToHex(colors);
        return  $"<color={hexColor}>{text}";
    }

    public static string ColorToHex(Color color)
    {
        int r = (int)(color.r * 255);
        int g = (int)(color.g * 255);
        int b = (int)(color.b * 255);
        int a = (int)(color.a * 255);

        string hex = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", r, g, b, a);

        return hex;
    }
}
