using UnityEngine;
using UnityEngine.UI;
using System;

public class UITimerBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image _fillImage;      // Image used as the timer fill

    [Header("Timer Settings")]
    [SerializeField] private float _defaultDuration = 10f;   // Default duration in seconds

    private float _duration;        // Current duration of the timer
    private float _timeRemaining;   // Time remaining in seconds
    private bool _isRunning;        // Is the timer currently running?

    public event Action OnTimerEnded;   // Event fired when the timer reaches zero

    private void Awake()
    {
        // Ensure a fill image is assigned
        if (_fillImage == null)
        {
            _fillImage = GetComponent<Image>();
        }

        // Initialize bar as empty or full as you prefer
        _fillImage.fillAmount = 1f;
    }

    private void Update()
    {
        if (!_isRunning) return;

        // Decrease remaining time
        _timeRemaining -= Time.deltaTime;

        // Clamp to zero
        if (_timeRemaining < 0f)
        {
            _timeRemaining = 0f;
        }

        // Update the fill amount based on remaining time
        if (_duration > 0f)
        {
            float normalized = _timeRemaining / _duration;
            _fillImage.fillAmount = normalized;
        }

        // Check for timer end
        if (_timeRemaining <= 0f && _isRunning)
        {
            _isRunning = false;
            OnTimerEnded?.Invoke();
        }
    }

    public void StartTimer()
    {
        StartTimer(_defaultDuration);
    }

    public void StartTimer(float durationInSeconds)
    {
        if (durationInSeconds <= 0f)
        {
            Debug.LogWarning("UITimerBar: Duration must be greater than zero.");
            return;
        }

        _duration = durationInSeconds;
        _timeRemaining = durationInSeconds;
        _isRunning = true;

        // Start with a full bar
        _fillImage.fillAmount = 1f;
    }

    public void StopTimer()
    {
        _isRunning = false;
    }

    public void ResetTimer()
    {
        // Reset to default duration
        _duration = _defaultDuration;
        _timeRemaining = _duration;

        _isRunning = false;

        if (_fillImage != null)
            _fillImage.fillAmount = 1f;
    }


}
