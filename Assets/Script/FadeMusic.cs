using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeMusic : MonoBehaviour
{
    private int fadeInTime = 3;
    private AudioSource audioSource;

    private bool fadeOut = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        fadeOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource.gameObject.name.Equals("MusicMenu"))
        {
            if (fadeOut)
            {
                if (audioSource.volume > 0)
                {
                    audioSource.volume -= (Time.deltaTime / (fadeInTime + 1));
                }
            }
            else
            {
                if (audioSource.volume < 0.3)
                {
                    audioSource.volume += (Time.deltaTime / (fadeInTime + 1));
                }
            }

        }

        if (audioSource.gameObject.name.Equals("MusicGame"))
        {
            if (audioSource.volume < 0.8)
            {
                audioSource.volume += (Time.deltaTime / (fadeInTime + 1));
            }
        }
        
    }

    public void SetFadeOut()
    {
        fadeOut = true;
    }
}
