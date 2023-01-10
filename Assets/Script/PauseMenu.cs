using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Image imageMusic;
    public Image imageEffects;

    public Sprite onMusic;
    public Sprite offMusic;
    public Sprite onEffects;
    public Sprite offEffects;

    public Text onOffMusicText;
    public Text onOffEffectsText;
    public GameObject PauseMenuPanel;

    // Start is called before the first frame update
    void Start()
    {
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

    public void PauseGame()
    {
        PauseMenuPanel.SetActive(true);
        SoundManager.StopBackgroundSound();
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        PauseMenuPanel.SetActive(false);
        if (PlayerPrefs.GetInt("music") != 0)
            SoundManager.PlayBackgroundSound();
        Time.timeScale = 1;
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("MenuScene");
        Time.timeScale = 1;
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

    private void SetOffMusic()
    {
        PlayerPrefs.SetInt("music", 0);
        imageMusic.sprite = offMusic;
        onOffMusicText.text = "Off";
    }

    private void SetOnMusic()
    {
        PlayerPrefs.SetInt("music", 1);
        imageMusic.sprite = onMusic;
        onOffMusicText.text = "On";
    }

    private void SetOffEffects()
    {
        PlayerPrefs.SetInt("effects_sound", 0);
        imageEffects.sprite = offEffects;
        onOffEffectsText.text = "Off";
    }

    private void SetOnEffects()
    {
        PlayerPrefs.SetInt("effects_sound", 1);
        imageEffects.sprite = onEffects;
        onOffEffectsText.text = "On";
    }
}
