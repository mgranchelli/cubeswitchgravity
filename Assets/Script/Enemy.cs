using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float speed = 2.2f;
    private Rigidbody2D rb;
    private Vector3 screenBounds;


    // Use this for initialization
    void Start()
    {
      
        rb = this.GetComponent<Rigidbody2D>();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

    }

    // Update is called once per frame
    void Update()
    {
        //rb.AddForce(Vector2.down * 0.5f);
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
}
