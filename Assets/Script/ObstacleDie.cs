using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDie : MonoBehaviour
{
    private Player Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Player.Die();
        }
            
    }
}
