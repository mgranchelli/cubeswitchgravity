using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float lockPos = 0f;
    private float lockPosY = 0f;
    private float speed = 2f;
    private Rigidbody2D rb;
    private Vector3 screenBounds;
    private float randomXPosition;
    private float velocita = 2;

    // Use this for initialization
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        
        if (transform.position.x > 0)
        {
            lockPosY = 180;
            randomXPosition = GetObstaclePositionX(gameObject.name)[1];
        }      
            
        else 
            randomXPosition = GetObstaclePositionX(gameObject.name)[0];

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(randomXPosition, rb.position.y);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lockPosY, lockPos);
        rb.velocity = new Vector2(0, -1 * speed);
        

        if (transform.position.y < screenBounds.y * -2)
        {
            Destroy(this.gameObject);
        }

        /*if (speed > velocita)
            print("Velocità aumentata: " + speed);*/
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public float[] GetObstaclePositionX(string obstacle)
    {
        float[] returnPosition = new float[2];
        switch (obstacle)
        {
            case "Obstacle1(Clone)":
                returnPosition[0] = (float)-1.83;
                returnPosition[1] = (float)1.83;
                break;
            case "Obstacle2(Clone)":
                returnPosition[0] = (float)-1.35;
                returnPosition[1] = (float)1.35;
                break;
            case "Obstacle3(Clone)":
                returnPosition[0] = (float)-1.936;
                returnPosition[1] = (float)1.936;
                break;
            default:
                break;
        }

        return returnPosition;
    }

}
