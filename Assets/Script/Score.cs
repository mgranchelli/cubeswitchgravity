using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private float lockPos = 0f;
    private float speed = 2f;
    private Rigidbody2D rb;
    private Vector3 screenBounds;
    private int score;
    private int highscore;

    public Score(int score)
    {
        this.score = score;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lockPos, lockPos);
        //print("Speed score: " + speed);
        rb.velocity = new Vector2(0, -1 * speed);

        if (transform.position.y < screenBounds.y * -2)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void IncreaseScore(int score)
    {
        this.score += score;
    }

    public int GetScore()
    {
        return this.score;
    }

}
