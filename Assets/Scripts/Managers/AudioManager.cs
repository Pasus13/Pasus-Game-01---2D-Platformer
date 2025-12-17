using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music Sources")]
    [SerializeField] private AudioSource _musicSource;   // For background music
    [SerializeField] private AudioSource _sfxSource;     // For sound effects

    [Header("Music Clips")]
    [SerializeField] private AudioClip _menuMusic;       // Music for main menu
    [SerializeField] private AudioClip _levelMusic;      // Music for gameplay
    [SerializeField] private AudioClip _victoryMusic;    // Music when final level is completed

    [Header("SFX Clips")]
    [SerializeField] private AudioClip _gameOverVFX;     // Game over VFX
    [SerializeField] private AudioClip _coinPickupSfx;   // When player collects a coin
    [SerializeField] private AudioClip _doorSfx;         // When player enters the door
    [SerializeField] private AudioClip _jumpSfx;         // First jump
    [SerializeField] private AudioClip _doubleJumpSfx;   // Second jump
    [SerializeField] private AudioClip _gameOverSfx;     // When time is up / game over

    private void Awake()
    {
        // Simple global singleton for audio only
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Keep audio manager through scenes
    }

    private void Start()
    {
        // If we start in the main menu (build index 0), play menu music
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.buildIndex == 0)
        {
            PlayMenuMusic();
        }
    }

    #region Music

    public void PlayMenuMusic()
    {
        PlayMusic(_menuMusic, true);
    }

    public void PlayLevelMusic()
    {
        if (_musicSource != null &&
            _musicSource.clip == _levelMusic &&
            _musicSource.isPlaying)
        {
            return; // No restart
        }

        PlayMusic(_levelMusic, true);
    }

    public void PlayVictoryMusic()
    {
        // Usually victory music is a jingle, no need to loop
        PlayMusic(_victoryMusic, false);
    }

    private void PlayMusic(AudioClip clip, bool loop)
    {
        if (clip == null || _musicSource == null)
            return;

        _musicSource.loop = loop;
        _musicSource.clip = clip;
        _musicSource.Play();
    }

    public void StopMusic()
    {
        if (_musicSource != null)
            _musicSource.Stop();
    }

    #endregion

    #region SFX

    public void PlayGameOverVFX()
    {
        PlaySfx(_gameOverVFX);
    }
    public void PlayCoinPickup()
    {
        PlaySfx(_coinPickupSfx);
    }

    public void PlayDoorEnter()
    {
        PlaySfx(_doorSfx);
    }

    public void PlayJump(bool isDoubleJump)
    {
        AudioClip clipToPlay = isDoubleJump ? _doubleJumpSfx : _jumpSfx;
        PlaySfx(clipToPlay);
    }

    public void PlayGameOverSfx()
    {
        PlaySfx(_gameOverSfx);
    }

    private void PlaySfx(AudioClip clip)
    {
        if (clip == null || _sfxSource == null)
            return;

        _sfxSource.PlayOneShot(clip);
    }

    #endregion
}
