using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public GameObject tutorialMenu;
    public Animator animator;
    private int levelToLoad;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeToMenu(int sceneIndex)
    {
        levelToLoad = sceneIndex;
        animator.SetTrigger("FadeOut");
        Time.timeScale = 1;
        Destroy(DataManager.Instance);
    }

    public void FadeToLevel(int sceneIndex)
    {
        if (PlayerPrefs.GetInt("tutorial") == 0)
        {
            tutorialMenu.SetActive(true);
            PlayerPrefs.SetInt("tutorial", 1);
        }
        else
        {
            levelToLoad = sceneIndex;
            animator.SetTrigger("FadeOut");
        }
        
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
