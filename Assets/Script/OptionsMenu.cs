using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Image imageMusic;
    public Image imageEffects;

    public Sprite onMusic;
    public Sprite offMusic;
    public Sprite onEffects;
    public Sprite offEffects;

    public Text onOffMusicText;
    public Text onOffEffectsText;

    public Sprite[] howToPlaySprite;
    public Image imageHowToPlay;
    public GameObject nextButton;
    public GameObject previousButton;
    public Text[] hTPTexts;

    private int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        previousButton.GetComponent<Image>().color = new Color32(0, 0, 0, 75);
        imageHowToPlay.sprite = howToPlaySprite[0];
        SetTextActive(-1);

        if (PlayerPrefs.GetInt("music") != 0)
        {
            onOffMusicText.text = "On";
            imageMusic.sprite = onMusic;
        }
        else
        {
            onOffMusicText.text = "Off";
            imageMusic.sprite = offMusic;
        }
            
        if (PlayerPrefs.GetInt("effects_sound") != 0)
        {
            onOffEffectsText.text = "On";
            imageEffects.sprite = onEffects;
        }
        else
        {
            onOffEffectsText.text = "Off";
            imageEffects.sprite = offEffects;
        }
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetOnOffMusic()
    {
        if (PlayerPrefs.GetInt("music") != 0)
            SetOffMusic();
        else
            SetOnMusic();
    }

    public void SetOnOffEffects()
    {
        if (PlayerPrefs.GetInt("effects_sound") != 0)
            SetOffEffects();
        else
            SetOnEffects();
    }

    public void SetOffMusic()
    {
        PlayerPrefs.SetInt("music", 0);
        imageMusic.sprite = offMusic;
        onOffMusicText.text = "Off";
        SoundManager.StopBackgroundSound();
    }

    public void SetOnMusic()
    {
        PlayerPrefs.SetInt("music", 1);
        imageMusic.sprite = onMusic;
        onOffMusicText.text = "On";
        SoundManager.PlayBackgroundSound();
    }

    public void SetOffEffects()
    {
        PlayerPrefs.SetInt("effects_sound", 0);
        imageEffects.sprite = offEffects;
        onOffEffectsText.text = "Off";
    }

    public void SetOnEffects()
    {
        PlayerPrefs.SetInt("effects_sound", 1);
        imageEffects.sprite = onEffects;
        onOffEffectsText.text = "On";
    }

    public void NextHTPImage()
    {
        
        if (i != howToPlaySprite.Length - 1)
        {
            i += 1;
        }

        if (i == howToPlaySprite.Length - 1)
            nextButton.GetComponent<Image>().color = new Color32(0, 0, 0, 75);

        SetTextActive(i - 1);
        imageHowToPlay.sprite = howToPlaySprite[i];
        previousButton.GetComponent<Image>().color = new Color32(0, 0, 0, 255);

    }

    public void PreviousHTPImage()
    {
        if (i != 0)
        {
            i -= 1;
        }

        if (i == 0)
            previousButton.GetComponent<Image>().color = new Color32(0, 0, 0, 75);

        SetTextActive(i - 1);
        imageHowToPlay.sprite = howToPlaySprite[i];
        nextButton.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
    }

    private void SetTextActive(int j)
    {
        for (int i = 0; i < hTPTexts.Length; i++)
        {
            if (i == j)
                hTPTexts[i].color = new Color32(0, 0, 0, 255);
            else
                hTPTexts[i].color = new Color32(50, 50, 50, 130);
        }
    }

    public void ResetText()
    {
        i = 0;
        SetTextActive(i - 1);
        imageHowToPlay.sprite = howToPlaySprite[i];
        nextButton.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
        previousButton.GetComponent<Image>().color = new Color32(0, 0, 0, 75);
    }
}
