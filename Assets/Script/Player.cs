using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Text scoreText;
    public GameObject DieMenu;
    public GameObject pauseMenu;
    private Score score = new Score(0);
    private double money;
    private Rigidbody2D rb;
    private float lockYRotation = 0;
    private bool right;
    private bool isGrounded;
    private float jumpSpeed = 5f;
    private float moveSpeed = 1f;

    public Sprite[] skins;

    private Vector3 screenBound;
    
    public LayerMask floor;
    private float distGround = -1.83f;
    private float posx;

    // Start is called before the first frame update
    void Start()
    {
        screenBound = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z)); ;
        SetSkinAndColor(PlayerPrefs.GetString("skin"), PlayerPrefs.GetString("color_skin"));
        rb = GetComponent<Rigidbody2D>();
        posx = -1.83f;
    }

    // Update is called once per frame
    void Update()
    {
        money += 0.1 * Time.deltaTime;
        if (money > 1)
        {
            score.IncreaseScore(1);
            money = 0;
        }
        scoreText.text = score.GetScore().ToString();

        transform.position = new Vector3(rb.position.x, -4);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lockYRotation, -90f);

        //print(Physics.Raycast(transform.position, Vector2.right, distGround));

        //isGrounded = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f), new Vector2(transform.position.x - 0.51f, transform.position.y + 0.5f), floor);
        

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded )
        {
            SoundManager.PlaySound("jump");
            Debug.Log("jump");
            if (!right)
                rb.velocity = Vector2.right * jumpSpeed;
            //rb.AddForce(new Vector2(jumpSpeed, 0f), ForceMode2D.Impulse);
            else
                rb.velocity = Vector2.left * jumpSpeed;
            //rb.AddForce(new Vector2(jumpSpeed * -1, 0f), ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (lockYRotation == 0)
                lockYRotation = 180;
            else
                lockYRotation = 0;

            SoundManager.PlaySound("switch");
            rb.gravityScale *= -1;
            Rotation();
        }

        if ((Input.touchCount > 0 || Input.GetMouseButton(0)) && !pauseMenu.activeSelf)
        {
            //Vector3 muousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);


            if ((touchPosition.x < screenBound.x / 2) && (touchPosition.y < screenBound.y / 2 - 4))
            {
                if (!right)
                {
                    if (isGrounded)
                    {
                        SoundManager.PlaySound("jump");
                        rb.velocity = Vector2.right * 5f;
                    }

                }
                else
                {
                    if (!isGrounded)
                    {
                        if (lockYRotation == 0)
                            lockYRotation = 180;
                        else
                            lockYRotation = 0;

                        SoundManager.PlaySound("switch");
                        rb.gravityScale *= -1;
                        Rotation();
                    }
                    
                }
            }
            else if (touchPosition.y < screenBound.y / 2)
            {
                if (right)
                {
                    if (isGrounded)
                    {
                        SoundManager.PlaySound("jump");
                        rb.velocity = Vector2.left * 5f;
                    }
                }
                else
                {
                    if (!isGrounded)
                    {
                        if (lockYRotation == 0)
                            lockYRotation = 180;
                        else
                            lockYRotation = 0;

                        SoundManager.PlaySound("switch");
                        rb.gravityScale *= -1;
                        Rotation();
                    }
                    
                }
            }

        }

    }

    void Rotation()
    {
        if (!right)
        {
            transform.eulerAngles = new Vector3(0, 0, 180);
        }
        else
        {
            transform.eulerAngles = Vector3.zero;
        }

        right = !right;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Obstacle"))
            isGrounded = true;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }

        if (collision.gameObject.CompareTag("Score"))
        {
            SoundManager.PlaySound("score");
            Destroy(collision.gameObject);
            score.IncreaseScore(1);
//            print("Score: " + score.GetScore());
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Obstacle"))
            isGrounded = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(new Vector2(transform.position.y, transform.position.x - 0.505f), new Vector2(2, 1f));
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(screenBound / 2, 0.5f);
    }

    private void SetSkinAndColor(string skin, string color_skin)
    {
        switch (skin)
        {
            case "Skin1":
                gameObject.GetComponent<SpriteRenderer>().sprite = skins[0];
                break;
            case "Skin2":
                gameObject.GetComponent<SpriteRenderer>().sprite = skins[1];
                break;
            case "Skin3":
                gameObject.GetComponent<SpriteRenderer>().sprite = skins[2];
                break;
            case "Skin4":
                gameObject.GetComponent<SpriteRenderer>().sprite = skins[3];
                break;
            case "Skin5":
                gameObject.GetComponent<SpriteRenderer>().sprite = skins[4];
                break;
            case "Skin6":
                gameObject.GetComponent<SpriteRenderer>().sprite = skins[5];
                break;

            default:
                gameObject.GetComponent<SpriteRenderer>().sprite = skins[0];
                break;

        }

        switch (color_skin)
        {
            case "Red":
                gameObject.GetComponent<SpriteRenderer>().color = SkinMenu.RedColor;
                break;
            case "Green":
                gameObject.GetComponent<SpriteRenderer>().color = SkinMenu.GreenColor;
                break;
            case "Yellow":
                gameObject.GetComponent<SpriteRenderer>().color = SkinMenu.YellowColor;
                break;
            case "Lightblue":
                gameObject.GetComponent<SpriteRenderer>().color = SkinMenu.LightBlueColor;
                break;
            case "Orange":
                gameObject.GetComponent<SpriteRenderer>().color = SkinMenu.OrangeColor;
                break;
            case "Purple":
                gameObject.GetComponent<SpriteRenderer>().color = SkinMenu.PurpleColor;
                break;

            default:
                gameObject.GetComponent<SpriteRenderer>().color = SkinMenu.RedColor;
                break;  

        }
    }

    public void Die()
    {
        print("Score: " + score.GetScore());
        SoundManager.StopBackgroundSound();
        SoundManager.PlaySound("die");
        if (PlayerPrefs.GetString("account") != "")
        {
            print(PlayerPrefs.GetInt("highscore"));
            if (PlayerPrefs.GetInt("highscore") < score.GetScore())
            {
                print("Update online highscore!");
                PlayerPrefs.SetInt("highscore", score.GetScore());
            }
            DataManager.Instance.UpdateHighscore();
            //DataManager.Instance.GetHighscoresTable();
        }
        DieMenu.SetActive(true);
        Time.timeScale = 0;
        Destroy(gameObject);
    }

    public int GetScore()
    {
        return score.GetScore();
    }

}
