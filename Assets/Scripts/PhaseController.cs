using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
struct Phases
{
    public String Name;
    public Sprite Sprite;
    public Color Colour;
    public float MoveSpeed;
    public float LifeTime;
    public Collider2D Collider;
    public float ScoreMultiplier;
}

public class PhaseController : MonoBehaviour
{
    [SerializeField] private Phases[] _phases;

    // Component references
    private SpriteRenderer _sprite;
    private Enemy _enemy;

    // Phase tracking
    public int CurrentPhase{ get; private set; }
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
        UpdatePhase(_nextPhase);
    }

    private int UpdatePhase(int newPhase)
    {
        _sprite.color = _phases[newPhase].Colour;
        _enemy.maxMoveSpeed = _phases[newPhase].MoveSpeed;
        _nextTransitionTime = _phases[newPhase].LifeTime;

        _nextPhase++;

        if (_nextPhase >= _phases.Length)
        {
            _hasReachedFinalEvolution = true;
            return CurrentPhase;
        }

        return newPhase;
    }

    void Update()
    {
        if (_hasReachedFinalEvolution) return;

        _timer += Time.deltaTime;

        if (_timer >= _nextTransitionTime)
        {
            _timer = 0f;
            CurrentPhase = UpdatePhase(_nextPhase);
        }
    }

    public float GetCurrentPhaseScoreMultiplier()
    {
        return _phases[CurrentPhase].ScoreMultiplier;
    }
}