using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public string NextScene;
    public string MainMenu;
    public string PlayerTag;
    public bool tutorial = true;
    public GameObject tutorialScreen;
    private int level = 0;

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
    public void LoadNext()
    {
        SceneManager.LoadScene(NextScene, LoadSceneMode.Single);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(MainMenu, LoadSceneMode.Single);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (tutorial) 
        {
            collision.gameObject.SetActive(false);
            tutorialScreen.SetActive(true);
        }
        else 
        {
            level += 1;
            saveGame();
            LoadNext();
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
