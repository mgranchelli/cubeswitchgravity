using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public GameObject music;
    private static AudioSource backgroundAudio;
    public static AudioClip jumpSound, switchSound, scoreSound, dieSound, buttonSound, closeButtonSound;
    static AudioSource audioSource;
    

    // Start is called before the first frame update
    void Start()
    {
        jumpSound = Resources.Load<AudioClip>("Sounds/JumpSound");
        switchSound = Resources.Load<AudioClip>("Sounds/SwitchSound");
        scoreSound = Resources.Load<AudioClip>("Sounds/ScoreSound");
        dieSound = Resources.Load<AudioClip>("Sounds/DieSound");
        buttonSound = Resources.Load<AudioClip>("Sounds/ButtonSound");
        closeButtonSound = Resources.Load<AudioClip>("Sounds/CloseButtonSound");
        audioSource = GetComponent<AudioSource>();
        backgroundAudio = music.GetComponent<AudioSource>();
        PlayMusic();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void StopBackgroundSound()
    {
        backgroundAudio.Stop();
    }
    public static void PlayBackgroundSound()
    {
        backgroundAudio.Play();
    }
    public static void PlaySound(string sound)
    {
        if (PlayerPrefs.GetInt("effects_sound") != 0)
        {
            switch (sound)
            {
                case "jump":
                    audioSource.PlayOneShot(jumpSound);
                    break;
                case "switch":
                    audioSource.PlayOneShot(switchSound);
                    break;
                case "score":
                    audioSource.PlayOneShot(scoreSound);
                    break;
                case "die":
                    audioSource.PlayOneShot(dieSound);
                    break;
                case "button":
                    audioSource.PlayOneShot(buttonSound);
                    break;
                case "close_button":
                    audioSource.PlayOneShot(closeButtonSound);
                    break;
            }
        }
    }

    public static void PlayMusic()
    {
        if (PlayerPrefs.GetInt("music") == 1)
        {
            backgroundAudio.Play();
        }
        else
            backgroundAudio.Stop();
    }

    public void PlaySoundButton()
    {
        PlaySound("button");
    }

    public void PlaySoundCloseButton()
    {
        PlaySound("close_button");
    }
}
