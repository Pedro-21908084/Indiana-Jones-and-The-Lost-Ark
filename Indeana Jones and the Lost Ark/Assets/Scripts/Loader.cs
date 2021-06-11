using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public string NextScene;
    public string MainMenu;
    public string PlayerTag;
    public bool tutorial = true;
    public GameObject tutorialScreen;
    public int level = 0;
    public bool loading;
    public GameObject image;
    public Sprite Image12;
    public Sprite Image23;
    public Sprite Image34;
    public Sprite Imagefinal;
    public bool boss;
    public GameObject Boss;

    private void Start()
    {
        loadGame();
    }

    private void Update()
    {
        if (loading)
        {
            loadGame();
            if (level == 2)
            {
                image.GetComponent<Image>().sprite = Image12;
            }
            else if (level == 3)
            {
                image.GetComponent<Image>().sprite = Image23;
            }
            else if (level == 4)
            {
                image.GetComponent<Image>().sprite = Image34;
            }
            else if (level == 5)
            {
                image.GetComponent<Image>().sprite = Imagefinal;
            }
        }

        if (boss && Boss == null) 
        {
            
            LoadNext();
        }
    }

    public void LoadNew() 
    {
        level = 1;
        saveGame();
        SceneManager.LoadScene(NextScene, LoadSceneMode.Single);
    }

    public void LoadOld() 
    {
        loadGame();
        SceneManager.LoadScene("Level " + level.ToString(), LoadSceneMode.Single);
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
    public void LoadNext()
    {
        loadGame();
        level += 1;
        saveGame();
        
        SceneManager.LoadScene(NextScene, LoadSceneMode.Single);
    }
    public void loadx() 
    {
        loadGame();
        if (level > 4)
        {
            LoadMainMenu();
        }
        SceneManager.LoadScene("Level " + level.ToString(), LoadSceneMode.Single);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(MainMenu, LoadSceneMode.Single);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == PlayerTag)
        {
            if (tutorial)
            {
                collision.gameObject.SetActive(false);
                tutorialScreen.SetActive(true);
            }
            else
            {
                //level += 1;
                //saveGame();
                LoadNext();
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void saveGame() 
    {
        PlayerPrefs.SetInt("Level",level);
    }

    public void loadGame() 
    {
        level = PlayerPrefs.GetInt("Level");
    }
}
