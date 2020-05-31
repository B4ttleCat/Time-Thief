using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// This Struct gives each phase it's own field in the inspector
/// so I can assign individual properties for each phase.
/// I generate the phases by creating an array of Structs
/// </summary>
[Serializable]
struct Phases
{
    public String Name;
    public Sprite Sprite;
    public Sprite DeathSprite;
    public Color Colour;
    public float MoveSpeed;
    public float LifeTime;
    public BoxCollider2D Collider;
    public float TimeToAdd;
    public float DamageMultiplier;

    public float DamageDealt
    {
        get { return TimeToAdd * DamageMultiplier; }
    }
}

public class PhaseController : MonoBehaviour
{
    [SerializeField] private Phases[] _phases;

    // Component references
    private SpriteRenderer _sprite;
    private Enemy _enemy;
    private BoxCollider2D _collider2D; // not entirely sure this is working properly
    private ParticleSystem _ps;
    
    // Phase tracking
    public int _nextPhase;
    public int CurrentPhase { get; private set; }
    private bool _hasReachedFinalEvolution;

    // Timers
    private float _timer;
    private float _nextTransitionTime;

    private void Awake()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _enemy = GetComponent<Enemy>();
        _collider2D = GetComponent<BoxCollider2D>();
        _ps = GetComponent<ParticleSystem>();
    }

    void Start()
    {
        UpdatePhase(_nextPhase);
    }

    void Update()
    {
        // Stop the Enemy changing phase anymore if it's at the last one
        if (_hasReachedFinalEvolution) return;

        // Update phase timer
        _timer += Time.deltaTime;

        // Change phase if timer exceeded transition time
        if (_timer >= _nextTransitionTime)
        {
            _timer = 0f;

            if (CurrentPhase >= _phases.Length)
            {
                _hasReachedFinalEvolution = true;
            }

            CurrentPhase = UpdatePhase(_nextPhase);
        }
    }

    private int UpdatePhase(int newPhase)
    {
        // Update attributes
        _enemy.maxMoveSpeed = _phases[newPhase].MoveSpeed;
        _nextTransitionTime = _phases[newPhase].LifeTime;
        _sprite.sprite = _phases[newPhase].Sprite;
        _collider2D = _phases[newPhase].Collider;
        var main = _ps.main;
        main.startColor = _phases[newPhase].Colour;
        
        // This doesn't work
        // todo move colliders to parent and enable/disable on phase change 
        _collider2D = _phases[newPhase].Collider;

        // Increment counter for next phase
        _nextPhase++;

        return newPhase;
    }

    public float GetCurrentPhaseScoreMultiplier()
    {
        return _phases[CurrentPhase].DamageDealt;
    }
    
    public void UseDeathSprite ()
    {
        _sprite.sprite = _phases[CurrentPhase].DeathSprite;
    }
}