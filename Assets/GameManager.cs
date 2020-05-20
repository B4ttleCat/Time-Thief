using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    [Header("Setup")]
    [SerializeField] private AudioClip startGameClip;
    [SerializeField] private float startGameDelay = 3f;
    [SerializeField] private Image gameStartMessage;
    
    private Camera _camera;
    
    private void Awake()
    {
        _camera = Camera.main;    
    }

    IEnumerator Start()
    {
        gameStartMessage.enabled = true;
        PauseGame();
        AudioSource.PlayClipAtPoint(startGameClip, transform.position);
        yield return new WaitForSecondsRealtime(startGameDelay);
        gameStartMessage.enabled = false;
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
    }

    private void RestartGame()
    {
        
    }

    private void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;
    }
}
