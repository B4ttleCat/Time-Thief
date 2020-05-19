using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private TimeBar _timeBar;
    [SerializeField] private int maxTime = 30;
    [SerializeField] private float InvincibilityTime = 0.66f;
    [SerializeField] private Color hurtColour;
    [SerializeField] private AudioClip playerHurt;
    [SerializeField] private AudioClip gameOver;
    
    private float _timer;
    private bool _canTakeDamage;  // Turn this to public for debugging with invincibility
    private bool _isGameOver;
    void Start()
    {
        _canTakeDamage = true;
        _timer = maxTime;
        _timeBar.SetMaxTime(maxTime);
    }

    void Update()
    {
        UpdateTimer();

        if (_timer <= 0f && !_isGameOver)
        {
            // todo Use Action or Event here?
            // Move this to a Game Manager
            _isGameOver = true;
            AudioSource.PlayClipAtPoint(gameOver, transform.position);
            Time.timeScale = 0f;
        }
    }

    private IEnumerator SetInvincibility()
    {
        _canTakeDamage = false;

        // Get sprite on Player
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();

        // Save original colour to restore it back to later
        Color startColour = sprite.color;

        // Change colour to show damage
        sprite.color = hurtColour;

        // Keep it on for a bit...
        yield return new WaitForSeconds(InvincibilityTime);

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
            StartCoroutine(SetInvincibility());
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
        UpdateTimer(-timeToSubtract);
    }

    public void AddTime(GameObject enemy, float timeAdjustment)
    {
        // Add it to the timer
        UpdateTimer(timeAdjustment);
    }

    // Use this in regular timer countdown
    private void UpdateTimer()
    {
        _timer -= Time.deltaTime;
        _timeBar.SetTime(_timer);
    }

    // Use this one when enemy damages player or player kills enemy
    private void UpdateTimer(float time)
    {
        _timer += time;
        if (_timer > maxTime) _timer = maxTime;
        _timeBar.SetTime(_timer);
    }
}