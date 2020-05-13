using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseController : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private int maxPhases;
    [SerializeField] private Color[] colours;
    
    [Header("Gameplay")]
    [SerializeField] private float[] speeds;
    [SerializeField] private float[] transitionTimes;
    
    // Component references
    private SpriteRenderer _sprite;
    private Enemy _enemy;
    
    // Phase tracking
    private int _currentPhase;
    private int _nextPhase;
    private bool _hasReachedFinalEvolution;
    
    // Timers
    private float _timer;
    private float _nextTransitionTime;
    

    private void Awake()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _enemy = GetComponent<Enemy>();
    }

    void Start()
    {
        // nextPhase  = 0 ... perfect
        UpdatePhase(_nextPhase);
    }

    private int UpdatePhase(int nextPhase)
    {
        // Set sprite colour
        _sprite.color = colours[nextPhase];
        
        // Set move speed
        _enemy.maxMoveSpeed = speeds[nextPhase];
        
        // Update next transition time
        _nextTransitionTime = transitionTimes[nextPhase];
        
        // Update phase index
        _nextPhase++;

        if (_nextPhase >= maxPhases)
        {
            _hasReachedFinalEvolution = true;
            return _currentPhase;
        }

        return nextPhase;
    }

    void Update()
    {
        if (_hasReachedFinalEvolution) return;

        _timer += Time.deltaTime;

        if (_timer >= _nextTransitionTime)
        {
            _timer = 0f;
            _currentPhase = UpdatePhase(_nextPhase);
        }
    }
}