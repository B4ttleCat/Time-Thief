using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private TimeBar _timeBar;
    [SerializeField] private int maxTime = 30;
    [SerializeField] private float _invincibilityTime = 0.66f;
    [SerializeField] private Color hurtColour;
    [SerializeField] private AudioClip playerHurt;
    
    private float _healthTimer;
    private bool _canTakeDamage;  // Turn this to public for debugging with invincibility
    private bool _isGameOver;
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        _canTakeDamage = true;
        _healthTimer = maxTime;
        _timeBar.SetMaxTime(maxTime);
    }

    void Update()
    {
        UpdateHealthTimer();

        if (_healthTimer <= 0f && !GameManager.IsGameOver)
        {
            // todo Use Action or Event here?
            _gameManager.GameOver();
        }
    }

    private IEnumerator MakeInvincible()
    {
        _canTakeDamage = false;

        // Get sprite on Player
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();

        // Save original colour to restore it back to later
        Color startColour = sprite.color;

        // Change colour to show damage
        sprite.color = hurtColour;

        // Keep it on for a bit...
        yield return new WaitForSeconds(_invincibilityTime);

        // turn sprite back to normal
        sprite.color = startColour;

        // set cantakedamage bool to true;
        _canTakeDamage = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && _canTakeDamage)
        {
            SubtractTime(other);
            StartCoroutine(MakeInvincible());
            AudioSource.PlayClipAtPoint(playerHurt, transform.position);
        }
    }

    // When Player is touches Enemy
    private void SubtractTime(Collision2D other)
    {
        // Get timeMultiplier value off current Phase of enemy
        PhaseController phaseController = other.gameObject.GetComponent<PhaseController>();
        float timeToSubtract = phaseController.GetCurrentPhaseScoreMultiplier();
        
        // subtract it from the timer
        UpdateHealthTimer(-timeToSubtract);
    }

    public void AddTime(GameObject enemy, float timeAdjustment)
    {
        // Add it to the timer
        UpdateHealthTimer(timeAdjustment);
    }

    // Use this in regular timer countdown
    private void UpdateHealthTimer()
    {
        _healthTimer -= Time.deltaTime;
        _timeBar.SetTime(_healthTimer);
    }

    // Use this one when enemy damages player or player kills enemy
    private void UpdateHealthTimer(float time)
    {
        _healthTimer += time;
        if (_healthTimer > maxTime) _healthTimer = maxTime;
        _timeBar.SetTime(_healthTimer);
    }
}