using UnityEngine;
using System;

public class Door : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _openTriggerName = "Open";

    private bool _isOpen;
    private bool _levelCompletedTriggered;

    public static event Action OnLevelCompleted;


    private void Start()
    {
        _levelCompletedTriggered = false;
        _isOpen = false;

        TrySubscribeToGameManager();
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnAllCoinsCollected -= HandleAllCoinsCollected;
        }
    }

    private void TrySubscribeToGameManager()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("[Door] GameManager.Instance is null. Cannot subscribe yet.");
            return;
        }

        // Unsubscribe first to avoid double subscription
        GameManager.Instance.OnAllCoinsCollected -= HandleAllCoinsCollected;
        GameManager.Instance.OnAllCoinsCollected += HandleAllCoinsCollected;
    }

    private void HandleAllCoinsCollected()
    {
        // Mark door as open
        _isOpen = true;


        // Trigger open animation if animator is assigned
        if (_animator != null && !string.IsNullOrEmpty(_openTriggerName))
        {
            _animator.SetTrigger(_openTriggerName);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isOpen || _levelCompletedTriggered) return;

        PlayerMovement player = other.GetComponentInParent<PlayerMovement>();

        if (player != null)
        {
            _levelCompletedTriggered = true; // Prevent multiple triggers

            OnLevelCompleted?.Invoke();
        }
    }
}

