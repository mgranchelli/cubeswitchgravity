using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGravity : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool top;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    // Update is called once per frame
    void Update()
    {
    }

    void Rotation()
    {
        if (!top)
        {
            transform.eulerAngles = new Vector3(0, 0, 180);
        }
        else
        {
            transform.eulerAngles = Vector3.zero;
        }

        top = !top;

    }
}
