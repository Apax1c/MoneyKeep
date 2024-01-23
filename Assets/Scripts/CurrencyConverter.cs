using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class CurrencyConverter : MonoBehaviour
{
    public static CurrencyConverter instance;

    private string apiUrl;

    //Currency api
    private string apiUrlUSD = "https://api.exchangerate-api.com/v4/latest/USD";
    private string apiUrlEUR = "https://api.exchangerate-api.com/v4/latest/EUR";
    private string apiUrlUAH = "https://api.exchangerate-api.com/v4/latest/UAH";
    private string apiUrlJPY = "https://api.exchangerate-api.com/v4/latest/JPY";
    private string apiUrlAUD = "https://api.exchangerate-api.com/v4/latest/AUD";
    private string apiUrlCAD = "https://api.exchangerate-api.com/v4/latest/CAD";
    private string apiUrlGBP = "https://api.exchangerate-api.com/v4/latest/GBP";
    private string apiUrlCHF = "https://api.exchangerate-api.com/v4/latest/CHF";
    private string apiUrlNOK = "https://api.exchangerate-api.com/v4/latest/NOK";
    private string apiUrlCNY = "https://api.exchangerate-api.com/v4/latest/CNY";
    private string apiUrlSEK = "https://api.exchangerate-api.com/v4/latest/SEK";

    //Currency rates
    private float exchangeRateToUSD = 1f;
    private float exchangeRateToEUR = 1f;
    private float exchangeRateToUAH = 1f;
    private float exchangeRateToJPY = 1f;
    private float exchangeRateToAUD = 1f;
    private float exchangeRateToCAD = 1f;
    private float exchangeRateToGBP = 1f;
    private float exchangeRateToCHF = 1f;
    private float exchangeRateToNOK = 1f;
    private float exchangeRateToCNY = 1f;
    private float exchangeRateToSEK = 1f;

    private void Awake()
    {
        instance = this;

        LoadLastSavedExchangeRate();

        SetMainCurrency(PlayerPrefs.GetString("MainCurrencyCode", "USD"));
    }

    void Start()
    {
        StartCoroutine(GetExchangeRate());
    }

    private void LoadLastSavedExchangeRate()
    {
        exchangeRateToUSD = PlayerPrefs.GetFloat("exchangeRateToUSD", 1f);
        exchangeRateToEUR = PlayerPrefs.GetFloat("exchangeRateToEUR", 1f);
        exchangeRateToUAH = PlayerPrefs.GetFloat("exchangeRateToUAH", 1f);
        exchangeRateToJPY = PlayerPrefs.GetFloat("exchangeRateToJPY", 1f);
        exchangeRateToAUD = PlayerPrefs.GetFloat("exchangeRateToAUD", 1f);
        exchangeRateToCAD = PlayerPrefs.GetFloat("exchangeRateToCAD", 1f);
        exchangeRateToGBP = PlayerPrefs.GetFloat("exchangeRateToGBP", 1f);
        exchangeRateToCHF = PlayerPrefs.GetFloat("exchangeRateToCHF", 1f);
        exchangeRateToNOK = PlayerPrefs.GetFloat("exchangeRateToNOK", 1f);
        exchangeRateToCNY = PlayerPrefs.GetFloat("exchangeRateToCNY", 1f);
        exchangeRateToSEK = PlayerPrefs.GetFloat("exchangeRateToSEK", 1f);
    }

    public IEnumerator GetExchangeRate()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            string jsonResult = request.downloadHandler.text;
            CurrencyApiResponse response = JsonUtility.FromJson<CurrencyApiResponse>(jsonResult);
            exchangeRateToUSD = response.rates.USD;
            exchangeRateToEUR = response.rates.EUR;
            exchangeRateToUAH = response.rates.UAH;
            exchangeRateToJPY = response.rates.JPY;
            exchangeRateToAUD = response.rates.AUD;
            exchangeRateToCAD = response.rates.CAD;
            exchangeRateToGBP = response.rates.GBP;
            exchangeRateToCHF = response.rates.CHF;
            exchangeRateToNOK = response.rates.NOK;
            exchangeRateToCNY = response.rates.CNY;
            exchangeRateToSEK = response.rates.SEK;

            PlayerPrefs.SetFloat("exchangeRateToUSD", exchangeRateToUSD);
            PlayerPrefs.SetFloat("exchangeRateToEUR", exchangeRateToEUR);
            PlayerPrefs.SetFloat("exchangeRateToUAH", exchangeRateToUAH);
            PlayerPrefs.SetFloat("exchangeRateToJPY", exchangeRateToJPY);
            PlayerPrefs.SetFloat("exchangeRateToAUD", exchangeRateToAUD);
            PlayerPrefs.SetFloat("exchangeRateToCAD", exchangeRateToCAD);
            PlayerPrefs.SetFloat("exchangeRateToGBP", exchangeRateToGBP);
            PlayerPrefs.SetFloat("exchangeRateToCHF", exchangeRateToCHF);
            PlayerPrefs.SetFloat("exchangeRateToNOK", exchangeRateToNOK);
            PlayerPrefs.SetFloat("exchangeRateToCNY", exchangeRateToCNY);
            PlayerPrefs.SetFloat("exchangeRateToSEK", exchangeRateToSEK);
        }
    }

    public float GetConvertedValue(float value, string currencyCode)
    {
        var convertedValue = currencyCode switch
        {
            "USD" => ConvertUSD(value),
            "EUR" => ConvertEUR(value),
            "UAH" => ConvertUAH(value),
            "JPY" => ConvertJPY(value),
            "AUD" => ConvertAUD(value),
            "CAD" => ConvertCAD(value),
            "GBP" => ConvertGBP(value),
            "CHF" => ConvertCHF(value),
            "NOK" => ConvertNOK(value),
            "CNY" => ConvertCNY(value),
            "SEK" => ConvertSEK(value),
            _ => ConvertUSD(value),
        };

        return convertedValue;
    }

    private void SetMainCurrency(string currency)
    {
        switch (currency)
        {
            case "USD":
                apiUrl = apiUrlUSD;
                break;
            case "EUR":
                apiUrl = apiUrlEUR;
                break;
            case "UAH":
                apiUrl = apiUrlUAH;
                break;
            case "JPY":
                apiUrl = apiUrlJPY;
                break;
            case "AUD":
                apiUrl = apiUrlAUD;
                break;
            case "CAD":
                apiUrl = apiUrlCAD;
                break;
            case "GBP":
                apiUrl = apiUrlGBP;
                break;
            case "CHF":
                apiUrl = apiUrlCHF;
                break;
            case "NOK":
                apiUrl = apiUrlNOK;
                break;
            case "CNY":
                apiUrl = apiUrlCNY;
                break;
            case "SEK":
                apiUrl = apiUrlSEK;
                break;
            default:
                apiUrl = apiUrlUSD;
                break;
        }
    }

    public float ConvertUSD(float usd)
    {
        return usd / exchangeRateToUSD;
    }

    public float ConvertEUR(float eur)
    {
        return eur / exchangeRateToEUR;
    }

    public float ConvertUAH(float uah)
    {
        return uah / exchangeRateToUAH;
    }
    
    public float ConvertJPY(float jpy)
    {
        return jpy / exchangeRateToJPY;
    }

    public float ConvertAUD(float aud)
    {
        return aud / exchangeRateToAUD;
    }

    public float ConvertCAD(float cad)
    {
        return cad / exchangeRateToCAD;
    }

    public float ConvertGBP(float gbp)
    {
        return gbp / exchangeRateToGBP;
    }

    public float ConvertCHF(float chf)
    {
        return chf / exchangeRateToCHF;
    }

    public float ConvertNOK(float nok)
    {
        return nok / exchangeRateToNOK;
    }

    public float ConvertCNY(float cny)
    {
        return cny / exchangeRateToCNY;
    }

    public float ConvertSEK(float sek)
    {
        return sek / exchangeRateToSEK;
    }

    [System.Serializable]
    public class CurrencyApiResponse
    {
        public Rates rates;
    }

    [System.Serializable]
    public class Rates
    {
        public float USD;
        public float EUR;
        public float UAH;
        public float JPY;
        public float AUD;
        public float CAD;
        public float GBP;
        public float CHF;
        public float NOK;
        public float CNY;
        public float SEK;
    }
}
