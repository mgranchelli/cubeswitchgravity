using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject dieMenu;
    public Text dieMenuHighscoreText;
    public Text dieMenuBestscoreText;
    private Player player;
    private int score = 1;
    private Obstacle[] obstacles;
    private Score[] scores;
    private float startSpeed = 2f;
    public float objectSpeed;
    public float speedToIncrease = 2f;
    public float enemySpeed;

    public float respawnObstacleTime = 1.5f;

    private static GameManager _Instance;
    public static GameManager Instance { get { return _Instance; } }
    //public PlayerSkin PlayerSkin;

    void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;

            print("Game manager");

            //DontDestroyOnLoad(gameObject);
            

        }

    }

    // Start is called before the first frame update
    void Start()
    {
        enemySpeed = 2.2f;
        objectSpeed = startSpeed;
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        obstacles = FindObjectsOfType<Obstacle>();
        scores = FindObjectsOfType<Score>();

        if (player.GetScore() > score)
        {
            IncreaseSpeedObject(0.2f);
            //spawnObstacles.IncreaseSpeed(0.5f);
            score += 3;
        }

        if (dieMenu.activeSelf)
        {
            TextLocalizer tl1 = dieMenuBestscoreText.GetComponent<TextLocalizer>();
            dieMenuBestscoreText.text = tl1.Traslate("best_score") + PlayerPrefs.GetInt("highscore");
            TextLocalizer tl2 = dieMenuHighscoreText.GetComponent<TextLocalizer>();
            dieMenuHighscoreText.text = tl2.Traslate("score") + player.GetScore();
        }
        
        
    }

    private void IncreaseSpeedObject(float increase)
    {
        objectSpeed += increase;
        //spawnObstacles.IncreaseSpeed(0.5f);
        print("Speed: " + objectSpeed);
        print("Value to intease: " + speedToIncrease);
        if (objectSpeed > speedToIncrease)
        {
            print("UPDATE RESPAWN TIME");
            respawnObstacleTime -= 0.2f;
            speedToIncrease += 2f;
            enemySpeed += 0.4f;

        }
        foreach (Obstacle obstacle in obstacles)
        {
            obstacle.SetSpeed(objectSpeed);
        }

        foreach (Score score in scores)
        {
            score.SetSpeed(objectSpeed);
        }
    }
}
