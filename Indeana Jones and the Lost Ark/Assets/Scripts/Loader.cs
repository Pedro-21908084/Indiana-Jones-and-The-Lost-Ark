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
            LoadNext();
        }
    }
}
