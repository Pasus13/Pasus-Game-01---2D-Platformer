using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Timer")]
    [SerializeField] private UITimerBar _timerBar;

    [Header("Coins & Level UI")]
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _coinsText;

    [Header("Panels")]
    [SerializeField] private GameObject _levelCompletePanel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _victoryPanel;
    [SerializeField] private GameObject _pausePanel;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void Start()
    {
        TrySubscribeToGameManager();
        HideAllPanels();
    }
 

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnCoinsChanged -= UpdateCoinsUI;
    }

    private void TrySubscribeToGameManager()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("[UIManager] GameManager.Instance is null. Cannot subscribe yet.");
            return;
        }

        // Unsubscribe first to avoid double-subscription
        GameManager.Instance.OnCoinsChanged -= UpdateCoinsUI;
        GameManager.Instance.OnCoinsChanged += UpdateCoinsUI;

        // Initialize UI with current value
        UpdateCoinsUI(GameManager.Instance.Coins);
        UpdateLevelText();
    }

    private void UpdateCoinsUI(int currentCoins)
    {
        if (_coinsText == null)
            return;

        int total = 0;

        if (GameManager.Instance != null)
        {
            total = GameManager.Instance.TotalCoinsInLevel();
        }

        _coinsText.text = $"{currentCoins} / {total}";
    }

    private void UpdateLevelText()
    {
        if (_levelText == null)
            return;

        int levelNumber = 1;

        if (GameManager.Instance != null)
        {
            levelNumber = GameManager.Instance.CurrentLevelNumber;
        }
        else
        {
            // Fallback if GameManager is not ready for some reason
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            levelNumber = Mathf.Max(1, buildIndex);
        }

        _levelText.text = $"LEVEL {levelNumber}";
    }

    public void RefreshLevelAndCoinsUI()
    {
        // Level
        UpdateLevelText();

        // Coins
        if (GameManager.Instance != null)
        {
            UpdateCoinsUI(GameManager.Instance.Coins);
        }
    }

    public void ShowLevelCompletePanel()
    {
        if (_levelCompletePanel != null)
            _levelCompletePanel.SetActive(true);

        if (_gameOverPanel != null)
            _gameOverPanel.SetActive(false);

        if (_victoryPanel != null)
            _victoryPanel.SetActive(false);

        if (_pausePanel != null)
            _pausePanel.SetActive(false);
    }

    public void ShowGameOverPanel()
    {
        if (_gameOverPanel != null)
            _gameOverPanel.SetActive(true);

        if (_levelCompletePanel != null)
            _levelCompletePanel.SetActive(false);

        if (_victoryPanel != null)
            _victoryPanel.SetActive(false);

        if (_pausePanel != null)
            _pausePanel.SetActive(false);
    }

    public void ShowVictoryPanel()
    {
        if (_gameOverPanel != null)
            _gameOverPanel.SetActive(false);

        if (_levelCompletePanel != null)
            _levelCompletePanel.SetActive(false);

        if (_victoryPanel != null)
            _victoryPanel.SetActive(true);

        if (_pausePanel != null)
            _pausePanel.SetActive(false);
    }

    public void ShowPausePanel()
    {
        if (_gameOverPanel != null)
            _gameOverPanel.SetActive(false);

        if (_levelCompletePanel != null)
            _levelCompletePanel.SetActive(false);

        if (_victoryPanel != null)
            _victoryPanel.SetActive(false);

        if (_pausePanel != null)
            _pausePanel.SetActive(true);
    }

    public void HideAllPanels()
    {
        if (_levelCompletePanel != null)
            _levelCompletePanel.SetActive(false);

        if (_gameOverPanel != null)
            _gameOverPanel.SetActive(false);

        if (_victoryPanel != null)
            _victoryPanel.SetActive(false);

        if (_pausePanel != null)
            _pausePanel.SetActive(false);
    }
}

