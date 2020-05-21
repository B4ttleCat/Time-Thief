using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    [Header("Setup")]
    [SerializeField] private AudioClip startGameClip;

    [SerializeField] private float startGameDelay = 3f;
    [SerializeField] private GameObject gameStartMessage;
    [SerializeField] private TextMeshProUGUI pausedMessage;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    IEnumerator Start()
    {
        PhaseController[] enemies = FindObjectsOfType<PhaseController>();
        
        foreach (PhaseController enemy in enemies)
        {
            Debug.Log(enemy.name);
        }
        
        // Enable/disable UI
        pausedMessage.enabled = false;
        gameStartMessage.SetActive(true);

        PauseStartGame();
        AudioSource.PlayClipAtPoint(startGameClip, transform.position);
        yield return new WaitForSecondsRealtime(startGameDelay);
        
        gameStartMessage.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);
        ResumeGame();
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

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private void PauseStartGame()
    {
        Time.timeScale = 0f;
        IsPaused = true;
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