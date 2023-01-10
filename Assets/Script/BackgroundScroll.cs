using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    private Material material;
    private Vector2 offset;

    public int xVelocity, yVelocity;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }


    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector2(xVelocity, yVelocity);
    }

    // Update is called once per frame
    void Update()
    {
        material.mainTextureOffset += offset * Time.deltaTime;
    }
}
