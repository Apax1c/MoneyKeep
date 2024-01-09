using System.Collections.Generic;

public static class TextColors
{
    public enum DefaultColorsEnum
    {
        Green,
        White,
        Red
    }

    public static Dictionary<DefaultColorsEnum, string> ColorsToStringList { get; private set; } = new Dictionary<DefaultColorsEnum, string>()
    {
        { DefaultColorsEnum.Green, "<color=#5EDEA9>" },
        { DefaultColorsEnum.White, "<color=#F6F6F6>" },
        { DefaultColorsEnum.Red, "<color=#ED808D>" },
    };

    public static string ApplyColorToText(DefaultColorsEnum colors, string text)
    {
        return ColorsToStringList[colors] + text;
    }
}
