using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CurrentMenu
{
    None,
    Pause,
    GameOver
}

/// <summary>
/// The class handles mainly menus that pop up in the game, that means the Pause and GameOverMenu
/// </summary>
public class IngameMenuController : MonoBehaviour
{
    [SerializeField] private ProceduralGenerationRoomGenerator gameGenerator;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] public UnityEvent onRestart;
    private CurrentMenu currentMenu = CurrentMenu.None;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentMenu == CurrentMenu.Pause)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        currentMenu = CurrentMenu.Pause;
        Time.timeScale = 0f; 
        pauseMenu.SetActive(true); 
    }
    public void ResumeGame()
    {
        currentMenu = CurrentMenu.None;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }
    public void OnRestartPress()
    {
        Time.timeScale = 1f;
        currentMenu = CurrentMenu.None;
        gameOverScreen.SetActive(false);
        onRestart.Invoke();
     //   gameGenerator.RunProceduralGeneration();
    }
    public void OnQuitPress()
    {
        Application.Quit();
    }

    public void OnDeathGameOverScreen()
    {
        currentMenu = CurrentMenu.GameOver;
    }
}
