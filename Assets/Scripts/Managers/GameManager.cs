using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver,
        LevelComplete
    }
    public GameState CurrentState { get; private set; }
    public int Coins { get; private set; }
    public int CurrentLevelNumber { get; private set; }

    public event Action<int> OnCoinsChanged;
    public event Action OnAllCoinsCollected;

    private int _totalCoinsInLevel;
    private float _levelStartTime = 10f;
    private bool _areAllCoinsCollected;
    
    [SerializeField] private bool _isLastLevel;
    [SerializeField] private UITimerBar _timer;

    private void Awake()
    {
        // Avoid having 2 GameManagers at the same time
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        CurrentState = GameState.Playing;
        _areAllCoinsCollected = false;

        Door.OnLevelCompleted += LevelCompleted;

        InitializeLevel();
    }

    private void Update()
    {
        HandlePauseInput();
    }

    private void OnDestroy()
    {
        Door.OnLevelCompleted -= LevelCompleted;
    }

    private void InitializeLevel()
    {
        // Reset game state for new level
        CurrentState = GameState.Playing;

        // Hide any panels from previous state
        if (UIManager.Instance != null)
        {
            UIManager.Instance.HideAllPanels();
        }

        // RESET COINS
        Coins = 0;
        _areAllCoinsCollected = false;
        OnCoinsChanged?.Invoke(Coins);

        //  COUNT COINS IN THIS SCENE
        Coin[] coinsInScene = FindObjectsByType<Coin>(FindObjectsSortMode.None);
        _totalCoinsInLevel = coinsInScene.Length;

        // CURRENT LEVEL
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        CurrentLevelNumber = Mathf.Max(1, buildIndex);

        if (_timer != null)
        {
            _timer.OnTimerEnded -= TimeEnded; // safety
        }

        _timer = FindAnyObjectByType<UITimerBar>();

        if (_timer != null)
        {
            _timer.OnTimerEnded += TimeEnded;
            _timer.StartTimer(_levelStartTime);
        } else
        {
            Debug.LogWarning("[GameManager] No UITimerBar found in this scene.");
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.RefreshLevelAndCoinsUI();
        }
    }

    private void TimeEnded()
    {
        // If we already are in a terminal state, do nothing
        if (CurrentState != GameState.Playing)
            return;

        CurrentState = GameState.GameOver;

        if (_timer != null)
        {
            _timer.StopTimer();
        }

        UIManager.Instance.ShowGameOverPanel();

        // Play Game Over audio
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameOverSfx();    // or PlayGameOverMusic()
        }
    }

    private void LevelCompleted()
    {
        // If we already are in a terminal state, do nothing
        if (CurrentState != GameState.Playing)
            return;


        CurrentState = GameState.LevelComplete;

        if (_timer != null)
            _timer.StopTimer();

        if (_isLastLevel)
        {
            UIManager.Instance.ShowVictoryPanel();

            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlayVictoryMusic();
        }
        else
        {
            UIManager.Instance.ShowLevelCompletePanel();
            AudioManager.Instance.PlayDoorEnter();
        }
    }

    public void PauseGame()
    {
        if (CurrentState != GameState.Playing)
            return;

        CurrentState = GameState.Paused;

        Time.timeScale = 0f;

        if (UIManager.Instance != null)
            UIManager.Instance.ShowPausePanel();
    }

    public void ResumeGame()
    {
        if (CurrentState != GameState.Paused)
            return;

        Time.timeScale = 1f;

        CurrentState = GameState.Playing;

        if (UIManager.Instance != null)
            UIManager.Instance.HideAllPanels();
    }

    private void HandlePauseInput()
    {
        if (CurrentState != GameState.Playing && CurrentState != GameState.Paused)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentState == GameState.Playing)
            {
                PauseGame();
            }
            else if (CurrentState == GameState.Paused)
            {
                ResumeGame();
            }
        }
    }


    public void AddCoins(int amount)
    {
        Coins += amount;

        // Notify all listeners that the coin amount has changed
        OnCoinsChanged?.Invoke(Coins);

        // Check if all coins have been collected
        if (!_areAllCoinsCollected && _totalCoinsInLevel > 0 && Coins >= _totalCoinsInLevel)
        {
            _areAllCoinsCollected = true;
            OnAllCoinsCollected?.Invoke();
        }
    }

    public void ResetCoins()
    {
        Coins = 0;
        _areAllCoinsCollected = false;

        // Notify listeners that coins were reset
        OnCoinsChanged?.Invoke(Coins);
    }

    public int TotalCoinsInLevel()
    {
        return _totalCoinsInLevel;
    }

}

