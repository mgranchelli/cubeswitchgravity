using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public Text userLoginName;
    public GameObject loginButton;
    public GameObject logoutButton;
    public GameObject loginMenu;
    public GameObject registerMenu;
    public GameObject optionsMenu;
    public GameObject FbImage;
    public GameObject checkItalian;
    public GameObject checkEnglish;
    public Text positionTextHS;
    public Text highscoreMenuText;
    public Text welcomeUserText;

    public string HighscoreJson;

    private int positionInRank;

    private static DataManager _Instance;
    public static DataManager Instance { get { return _Instance; } }

    void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;

            print("Data manager");
           
            ParseData();
            DontDestroyOnLoad(gameObject);
            

        }

    }

    public void ParseData()
    {   
        int hasPlayed = PlayerPrefs.GetInt("HasPlayed");

        if (hasPlayed == 0)
        {
            // Firts Time
            PlayerPrefs.SetInt("tutorial", 0);
            PlayerPrefs.SetInt("effects_sound", 1);
            PlayerPrefs.SetInt("music", 1);
            PlayerPrefs.SetString("language", "english");
            PlayerPrefs.SetString("skin", "Skin4");
            PlayerPrefs.SetString("color_skin", "Red");
            PlayerPrefs.SetInt("HasPlayed", 1);
            SetLogin(PlayerPrefs.GetString("account"));
            GetHighscoresTable();
            SetCheckLanguage();
        }
        else
        {
            //string jsonPlayerSkin = PlayerPrefs.GetString("playerSkin");
            //PlayerSkin = JsonUtility.FromJson<PlayerSkin>(jsonPlayerSkin);
            //Debug.Log("JSON: " + jsonPlayerSkin);
            SetLogin(PlayerPrefs.GetString("account"));
            GetHighscoresTable();
            SetCheckLanguage();
        }
    }

    public void SetAccount(string accountName)
    {
        PlayerPrefs.SetString("account", accountName);
    }

    private void SetLogin(string accountName)
    {
        TextLocalizer tlMenu = welcomeUserText.GetComponent<TextLocalizer>();
        TextLocalizer tl = userLoginName.GetComponent<TextLocalizer>();
        
        print("Account name: " + accountName);
        if (accountName != "")
        {
            welcomeUserText.text = tlMenu.Traslate("hi") + accountName;
            userLoginName.text = tl.Traslate("logged_in") + accountName;
            loginButton.SetActive(false);
            logoutButton.SetActive(true);
            int bestScore = PlayerPrefs.GetInt("highscore");
            if (bestScore != 0)
                highscoreMenuText.text = "Best score: " + bestScore;
            else
                highscoreMenuText.text = "";
        }
            
        else
        {
            welcomeUserText.text = "";
            highscoreMenuText.text = "";
            userLoginName.text = "Not login";
            loginButton.SetActive(true);
            logoutButton.SetActive(false);
        }
            
    }

    public void Logout()
    {
        userLoginName.text = "Not login";
        loginButton.SetActive(true);
        logoutButton.SetActive(false);
        PlayerPrefs.SetInt("highscore", 0);
        PlayerPrefs.SetString("account", null);
        GetHighscoresTable();
        highscoreMenuText.text = "";
        welcomeUserText.text = "";
        SetHighscoreLanguage(0);
    }

    public void SingInSuccess()
    {
        registerMenu.SetActive(false);
        FbImage.SetActive(false);
        loginMenu.SetActive(false);
        optionsMenu.SetActive(true);
        GetHighscoresTable();
        SetLogin(PlayerPrefs.GetString("account"));
    }

    public void SingInFBSuccess()
    {
        FbImage.SetActive(true);
        loginMenu.SetActive(false);
        optionsMenu.SetActive(true);
        SetLogin(PlayerPrefs.GetString("account"));
        GetHighscoresTable();
    }

    public void GetHighscoresTable()
    {
        StartCoroutine(GetHighscoresTableRequest((UnityWebRequest req) =>
        {
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.Log("Cannot connect!");
            }
            else
            {
                HighscoreJson = req.downloadHandler.text;
                SetHighscorePref();
                SetLogin(PlayerPrefs.GetString("account"));
                SetHighscoreLanguage(positionInRank);
            }

        }));
    }



    IEnumerator GetHighscoresTableRequest(Action<UnityWebRequest> callback)
    {
        string UpdateHighscoreURL = "https://gmanu.altervista.org/Game/GetHighscores.php";

        UnityWebRequest UpdateHighscoreRequest = UnityWebRequest.Get(UpdateHighscoreURL);

        yield return UpdateHighscoreRequest.SendWebRequest();
        callback(UpdateHighscoreRequest);
    }

    public string ReturnHighscore(string json)
    {
        Debug.Log(json);
        return json;
    }


    public void UpdateHighscore()
    {
        StartCoroutine(UpdateHighscoreRequest((UnityWebRequest req) =>
        {
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.Log("Cannot connect!");
            }
            else
            {
                string UpdateHighscoreReturn = req.downloadHandler.text;
                if (UpdateHighscoreReturn.Equals("Update Success"))
                    Debug.Log("Highscore Updated!");

                else if (UpdateHighscoreReturn.Equals("Insert Success"))
                {
                    Debug.Log("New Highscore Insert!");
                }
                else
                {
                    Debug.Log("Error! " + UpdateHighscoreReturn);
                }
            }

        }));

    }



    IEnumerator UpdateHighscoreRequest(Action<UnityWebRequest> callback)
    {
        WWWForm Form = new WWWForm();
        string UpdateHighscoreURL = "https://gmanu.altervista.org/Game/UpdateHighscore.php";

        Form.AddField("Username", PlayerPrefs.GetString("account"));
        Form.AddField("Score", PlayerPrefs.GetInt("highscore"));

        UnityWebRequest UpdateHighscoreRequest = UnityWebRequest.Post(UpdateHighscoreURL, Form);

        yield return UpdateHighscoreRequest.SendWebRequest();
        callback(UpdateHighscoreRequest);
    }

    private void SetHighscorePref()
    {
        HighscoreList HighscoresList = JsonUtility.FromJson<HighscoreList>(FixJson(HighscoreJson));

        //highscoresList.Sort();
        foreach(Highscore highscore in HighscoresList.highscoresList)
        {
            if (highscore.Username.Equals(PlayerPrefs.GetString("account")))
            {
                PlayerPrefs.SetInt("highscore", Convert.ToInt32(highscore.Score));
            }
               
        }

        Debug.Log("Player: " + PlayerPrefs.GetString("account") + " with " + PlayerPrefs.GetInt("highscore") + " score!");
    }

    string FixJson(string value)
    {
        value = "{\"highscoresList\":" + value + "}";
        return value;
    }

    public void SetItalianLanguage()
    {
        PlayerPrefs.SetString("language", "italian");
        SetCheckLanguage();
        SetLogin(PlayerPrefs.GetString("account"));
        SetHighscoreLanguage(positionInRank);
    }

    public void SetEnglishLanguage()
    {
        PlayerPrefs.SetString("language", "english");
        SetCheckLanguage();
        SetLogin(PlayerPrefs.GetString("account"));
        SetHighscoreLanguage(positionInRank);
    }

    public void SetCheckLanguage()
    {
        string language = PlayerPrefs.GetString("language");
        if (language.Equals("") || language.Equals("english"))
        {
            checkEnglish.SetActive(true);
            checkItalian.SetActive(false);
        }
        else
        {
            checkEnglish.SetActive(false);
            checkItalian.SetActive(true);
        }
    }

    public void SetHighscoreLanguage(int positionAccount)
    {
        positionInRank = positionAccount;
        TextLocalizer tl = positionTextHS.GetComponent<TextLocalizer>();

        if (positionAccount != 0)
        {
            string textPosition = tl.Traslate("position_in_rank");
            textPosition = textPosition.Replace("x", positionAccount.ToString());
            positionTextHS.text = textPosition;
        }
       

        print("posizione account: " + positionAccount);
        if (PlayerPrefs.GetString("account").Equals(""))
            positionTextHS.text = tl.Traslate("sing_up_to_rank");
        else if (positionAccount == 0)
            positionTextHS.text = tl.Traslate("play_to_rank");
    }

    private class HighscoreList
    {
        public List<Highscore> highscoresList;
    }

}
