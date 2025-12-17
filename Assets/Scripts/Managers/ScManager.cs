using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class ScManager : MonoBehaviour
{
    public static ScManager Instance { get; private set; }

    private void Awake()
    {
        // Simple singleton per scene
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ReloadCurrentLevel()
    {
        Time.timeScale = 1f;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlayLevelMusic();
        }

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex);
    }

    public void LoadNextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.LogWarning("[ScManager] No next scene in build settings. This is the last level.");
        }
    }

    public void LoadStartNewGameScene()
    {
        // We are leaving the main menu and entering gameplay
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();      // stop menu music
            AudioManager.Instance.PlayLevelMusic(); // start gameplay loop
        }
        SceneManager.LoadScene(1); // Scene 01 (first level)
    }

    public void LoadMainMenuScene()
    {
        Time.timeScale = 1f;

        // We are going back to the main menu
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlayMenuMusic();
        }
        SceneManager.LoadScene(0); // Main Menu
    }

    public void OnPauseResumeButton()
    {
        // Resume gameplay
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResumeGame();
        }
    }

    public void OnPauseRetryButton()
    {
        // Reload current level from pause menu
        ReloadCurrentLevel();
    }

    public void OnPauseMainMenuButton()
    {
        // Go back to main menu from pause menu
        LoadMainMenuScene();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

