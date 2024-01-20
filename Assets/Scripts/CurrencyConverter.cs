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
    private float exchangeRateToUSD = 0f;
    private float exchangeRateToEUR = 0f;
    private float exchangeRateToUAH = 0f;
    private float exchangeRateToJPY = 0f;
    private float exchangeRateToAUD = 0f;
    private float exchangeRateToCAD = 0f;
    private float exchangeRateToGBP = 0f;
    private float exchangeRateToCHF = 0f;
    private float exchangeRateToNOK = 0f;
    private float exchangeRateToCNY = 0f;
    private float exchangeRateToSEK = 0f;

    private void Awake()
    {
        instance = this;
    }

    

    void Start()
    {
        SetMainCurrency(PlayerPrefs.GetString("MainCurrency", "$"));

        StartCoroutine(GetExchangeRate());
    }

    public IEnumerator GetExchangeRate()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrlUSD);
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
        }
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
