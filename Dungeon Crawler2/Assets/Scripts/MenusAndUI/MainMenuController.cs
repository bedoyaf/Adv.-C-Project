using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("start game");
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
