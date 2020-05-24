using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    private static float _gameTimer;
    private static float _highScore;

    // Start is called before the first frame update
    void Start()
    {
        _gameTimer = 0f;
        UpdateHighScoreText(GetHighScore());
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.IsGameOver)
        {
            UpdateGameTimer();
        }
        else
        {
            CheckHighScore(_gameTimer);
        }
    }

    private void UpdateGameTimer()
    {
        _gameTimer = Time.timeSinceLevelLoad * 1000;
        _scoreText.text = _gameTimer.ToString("F3");
    }

    private void CheckHighScore(float gameTotalTime)
    {
        if (gameTotalTime > GetHighScore())
        {
            SetHighScore(gameTotalTime);
            UpdateHighScoreText(gameTotalTime);
        }
    }
    
    private void UpdateHighScoreText(float newHighScore)
    {
        highScoreText.text = "HI " + newHighScore.ToString();
    }

    public void SetHighScore(float newHighScore)
    {
        PlayerPrefs.SetFloat("highScore", newHighScore);
    }

    public float GetHighScore()
    {
        return PlayerPrefs.GetFloat("highScore", _highScore);
    }

}