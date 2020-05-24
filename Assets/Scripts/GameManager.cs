using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool IsPaused { get; private set; }
    public static bool IsGameOver  { get; private set; }
    
    [Header("Setup")]
    [SerializeField] private AudioClip startGameClip;
    [SerializeField] private AudioClip gameOverClip;

    [SerializeField] private float startGameDelay = 3f;
    [SerializeField] private GameObject gameStartMessage;
    [SerializeField] private TextMeshProUGUI pausedMessage;

    private Camera _camera;
    private AudioSource _audioSource;
    private ScoreKeeper _scoreKeeper;

    private void Awake()
    {
        _camera = Camera.main;
        _audioSource = GetComponent<AudioSource>();
        _scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    IEnumerator Start()
    {
        // Enable/disable UI
        pausedMessage.enabled = false;
        gameStartMessage.SetActive(true);

        PauseStartGame();
        
        PlayAudioClip(startGameClip);

        yield return new WaitForSecondsRealtime(startGameDelay);

        gameStartMessage.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);

        ResumeGame();
    }

    private void PlayAudioClip(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (IsPaused == false) PauseGame();
            else ResumeGame();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //Make this a timed double tap to prevent accidental game restart
            RestartGame();
        }
    }

    public void GameOver()
    {
        IsGameOver = true;
        Time.timeScale = 0f;
        
        PlayAudioClip(gameOverClip);
        
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void PauseStartGame()
    {
        IsPaused = true;
        Time.timeScale = 0f;
    }

    private void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0f;
        pausedMessage.enabled = true;
    }

    private void ResumeGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        pausedMessage.enabled = false;
    }
}