using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTableFB : MonoBehaviour
{
    private Transform entryContainer;
    private Transform highscoreTemplate;

    //private List<Highscore> highscoreList;
    private List<Transform> highscoreEntryTrasformList;


    private void Awake()
    {
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

        highscoresJson.highscoresList.Sort();

        highscoreEntryTrasformList = new List<Transform>();

        foreach (Highscore highscore in highscoresJson.highscoresList)
        {
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
            case 1: rankString = "1TH"; break;
            case 2: rankString = "2TH"; break;
            case 3: rankString = "3TH"; break;

            default: rankString = rank + "TH"; break;
        }
        print("rankString: " + rankString);
        entryTrasform.Find("PositionText").GetComponent<Text>().text = rankString;

        string score = highscore.Score;
        entryTrasform.Find("ScoreText").GetComponent<Text>().text = score;

        string user = highscore.Username;
        entryTrasform.Find("UsernameText").GetComponent<Text>().text = user;


        //entryTrasform.Find("background").gameObject.SetActive(rank % 2 == 1);

        // Colore verde primo classificato
        if (rank == 1)
        {
            entryTrasform.Find("PositionText").GetComponent<Text>().color = Color.green;
            entryTrasform.Find("ScoreText").GetComponent<Text>().color = Color.green;
            entryTrasform.Find("UsernameText").GetComponent<Text>().color = Color.green;

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
