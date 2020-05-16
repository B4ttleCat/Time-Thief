using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private TimeBar _timeBar;
    [SerializeField] private int maxTime = 30;

    private float _timer;

    void Start()
    {
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

    private void UpdateTimer()
    {
        _timer -= Time.deltaTime;
        _timeBar.SetTime(_timer);
    }

    private void UpdateTimer(float time)
    {
        _timer += time;
        _timeBar.SetTime(_timer);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            SubtractTime(other);
        }
    }

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

    public void AddTime(GameObject enemy)
    {        
        // Get current phase of enemy
        PhaseController phaseController = enemy.GetComponent<PhaseController>();
        
        // Get timeMultiplier value off current Phase of enemy
        float timeToAdd = phaseController.GetCurrentPhaseScoreMultiplier();

        // Add it to the timer
        UpdateTimer(timeToAdd);
    }
}