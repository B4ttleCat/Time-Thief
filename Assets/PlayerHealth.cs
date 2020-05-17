using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private TimeBar _timeBar;
    [SerializeField] private int maxTime = 30;
    [SerializeField] private float InvincibilityTime = 0.66f;
    private float _timer;
    private bool _canTakeDamage;  // Turn this to public for debugging with invincibility

    void Start()
    {
        _canTakeDamage = true;
        _timer = maxTime;
        _timeBar.SetMaxTime(maxTime);
    }

    void Update()
    {
        UpdateTimer();

        if (_timer <= 0f)
        {
            // Use Action or Event here?
            Debug.Log("Game over, man!");
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
        sprite.color = Color.red;

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
        }
    }

    // When Player is touches Enemy
    private void SubtractTime(Collision2D other)
    {
        // Get current phase of enemy
        int currentPhase = other.gameObject.GetComponent<PhaseController>().CurrentPhase;

        // Get timeMultiplier value off current Phase of enemy
        PhaseController phaseController = other.gameObject.GetComponent<PhaseController>();
        float timeToSubtract = phaseController.GetCurrentPhaseScoreMultiplier();

        // subtract it from the timer
        UpdateTimer(-timeToSubtract);
    }

    // todo check/fix for when enemy phase is a penalty score (deceased)
    // When Player shoots Enemy
    public void AddTime(GameObject enemy, float timeAdjustment)
    {
        Debug.Log(timeAdjustment);
        // Get current phase of enemy
        PhaseController phaseController = enemy.GetComponent<PhaseController>();
        
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