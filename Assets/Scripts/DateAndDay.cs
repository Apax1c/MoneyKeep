using System;
using UnityEngine;

public class DateAndDay : MonoBehaviour
{
    private void Start()
    {
        // Get the current date and time
        DateTime currentDate = DateTime.Now;

        // Format the date as "dd.MM.yyyy"
        string formattedDate = currentDate.ToString("dddd, "+ "dd.MM.yy");

        formattedDate = char.ToUpper(formattedDate[0]) + formattedDate.Substring(1);

        // Output the formatted date and day of the week
        Debug.Log("Formatted Date: " + formattedDate);
    }
}
