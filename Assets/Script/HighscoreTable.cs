using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour
{
    public Text positionText;
    private Transform entryContainer;
    private Transform highscoreTemplate;
    private int i = 0;
    private int positionAccount = 0;
    private List<Highscore> listaHighscore;
  

    //private List<Highscore> highscoreList;
    private List<Transform> highscoreEntryTrasformList;


    private void Awake()
    {
        

        print("Lo apro");
        entryContainer = transform.Find("HighscoresContainer");
        highscoreTemplate = entryContainer.Find("HighscoresTemplate");
        string newJson = FixJson(DataManager.Instance.HighscoreJson);

        highscoreTemplate.gameObject.SetActive(false);

        HighscoreList highscoresJson = JsonUtility.FromJson<HighscoreList>(newJson);

        /*
        highscoreList = new List<Highscore>()
        {
            new Highscore("Luca", 100),
            new Highscore("Alex", 300),
            new Highscore("Frank", 500),
            new Highscore("Alan", 100),
            new Highscore("Lucano", 3400),
            new Highscore("lucaluc", 2300),
            new Highscore("Alexandro", 6500),
            new Highscore("ewraek", 7800),
            new Highscore("Alaaaan", 3400),
            new Highscore("Luka_no", 3700)
        };*/

        listaHighscore = highscoresJson.highscoresList;
        highscoresJson.highscoresList.Sort();

        highscoreEntryTrasformList = new List<Transform>();

        foreach (Highscore highscore in highscoresJson.highscoresList)
        {
            //i++;
            /*
            if (highscore.Username.Equals(PlayerPrefs.GetString("account")))
            {
                //positionAccount = i;
                positionText.text = "You are in " + positionAccount + "° position";
            }*/
            CreateHighscoreEntryTrasform(highscore, entryContainer, highscoreEntryTrasformList);
        }

        /*
        HighscoreList highscoreListClass = new HighscoreList { highscoresList = highscoreList };
        string json = JsonUtility.ToJson(highscoreListClass);
        Debug.Log(json);
        // Salva json all'interno del gioco
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
        */


    }

    private void Start()
    {
        print("start");
        

        foreach (Highscore highscore in listaHighscore)
        {
            i++;
            if (highscore.Username.Equals(PlayerPrefs.GetString("account")))
            {
                positionAccount = i;
                DataManager.Instance.SetHighscoreLanguage(positionAccount);
            }
        }

    }

    private void CreateHighscoreEntryTrasform(Highscore highscore, Transform container, List<Transform> trasformList)
    {
        // Template classifica
        float templateHeight = 300f;
        Transform entryTrasform = Instantiate(highscoreTemplate, container);
        RectTransform entryRectTransform = entryTrasform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * trasformList.Count);
        entryTrasform.gameObject.SetActive(true);

        int rank = trasformList.Count + 1;
        string rankString;

        switch (rank)

        {
            case 1: rankString = "1."; break;
            case 2: rankString = "2."; break;
            case 3: rankString = "3."; break;

            default: rankString = rank + "."; break;
        }
        entryTrasform.Find("PositionText").GetComponent<Text>().text = rankString;

        string score = highscore.Score;
        entryTrasform.Find("ScoreText").GetComponent<Text>().text = score;

        string user = highscore.Username;

        if (user.Equals(PlayerPrefs.GetString("account")))
        {
            entryTrasform.Find("PositionText").GetComponent<Text>().color = Color.green;
            entryTrasform.Find("ScoreText").GetComponent<Text>().color = Color.green;
            entryTrasform.Find("UsernameText").GetComponent<Text>().color = Color.green;
        }
        entryTrasform.Find("UsernameText").GetComponent<Text>().text = user;


        //entryTrasform.Find("background").gameObject.SetActive(rank % 2 == 1);

        // Colore verde primo classificato
        if (rank == 1)
        {
            entryTrasform.Find("PositionText").GetComponent<Text>().color = Color.yellow;
            entryTrasform.Find("ScoreText").GetComponent<Text>().color = Color.yellow;
            entryTrasform.Find("UsernameText").GetComponent<Text>().color = Color.yellow;

        }
        /*
        // Colore medaglie primi tre classificati
        switch (rank)
        {
            case 1: entryTrasform.Find("Medal").GetComponent<Image>().color = Color.yellow; break;
            case 2: entryTrasform.Find("Medal").GetComponent<Image>().color = Color.gray; break;
            case 3: entryTrasform.Find("Medal").GetComponent<Image>().color = Color.magenta; break;

            default: entryTrasform.Find("Medal").gameObject.SetActive(false); break;
        }*/

        trasformList.Add(entryTrasform);
    }

    public class HighscoreList
    {
        public List<Highscore> highscoresList;
    }

    string FixJson(string value)
    {
        value = "{\"highscoresList\":" + value + "}";
        return value;
    }
}
