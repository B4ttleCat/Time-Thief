using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject _target;
    
    [Header("Gameplay")]
    public float maxMoveSpeed;

    [SerializeField] private float enemyDeathDelay;
    [SerializeField] private float kickBackForce = 20f;

    // Enemy
    private Rigidbody2D _rb;
    private SpriteRenderer _enemySprite;
    private Collider2D _col;

    // FX
    private ParticleSystem _particleSystem;

    // Target
    private Vector3 _targetPos;
    private Vector2 _targetDirection;

    // Death
    private bool _isDead;
    private bool _hasHitWall;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
        _particleSystem = GetComponent<ParticleSystem>();
        _enemySprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        _target = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        #region attempted physics movement for enemy

        // _targetPos = _target.transform.position;
        // Vector2 targetDirection = _targetPos - transform.position;
        // Vector2 forceToBeApplied;
        //
        // // This chunk is to slow the object down under max speed
        // float speed = Vector3.Magnitude(_rb.velocity); // test current object speed
        //
        // if (speed > _currentMaxMoveSpeed)
        // {
        //     float brakeSpeed = speed - _currentMaxMoveSpeed; // calculate the speed decrease
        //
        //     Vector3 normalisedVelocity = _rb.velocity.normalized;
        //     Vector3 brakeVelocity = normalisedVelocity * brakeSpeed; // make the brake Vector3 value
        //
        //     _rb.AddForce(-brakeVelocity); // apply opposing brake force
        // }
        //
        // else
        // {
        //     forceToBeApplied = new Vector2(targetDirection.x * _currentMaxMoveSpeed * Time.deltaTime,
        //         targetDirection.y * _currentMaxMoveSpeed * Time.deltaTime);
        //
        //     _rb.AddForce(forceToBeApplied, ForceMode2D.Force); // apply opposing brake force}
        // }

        #endregion

        if (_isDead == false)
        {
            MoveEnemy(1f);
            CheckSpriteXDirection();
        }

        else if (_isDead && _hasHitWall == false)
        {
            MoveEnemy(-kickBackForce);
            DeathWallCollisionCheck();
        }
    }

    // todo IMPORTANT Stop enemy pushing player through wall 
    // todo check if enemy is in deceased phase and turn to static so it can't be bumped
    private void MoveEnemy(float directionMultiplier)
    {
        _targetPos = _target.transform.position;

        // Allows two phases that don't move to have kickback
        if (maxMoveSpeed < 1 && _isDead) maxMoveSpeed = 1;

        transform.position = Vector2.MoveTowards(transform.position, _targetPos,
            maxMoveSpeed * directionMultiplier * Time.deltaTime);
    }

    private IEnumerator DeathDelayTimer()
    {
        yield return new WaitForSeconds(enemyDeathDelay);
    }

    private void CheckSpriteXDirection()
    {
        _targetDirection = (_targetPos - transform.position);
        if (_targetDirection.x > 0f)
        {
            _enemySprite.flipX = false;
        }
        else if (_targetDirection.x < 0f)
        {
            _enemySprite.flipX = true;
        }
    }

    // This is an alternate way to doing it through OnCollisionEnter
    private void DeathWallCollisionCheck()
    {
        if (_col.IsTouchingLayers(LayerMask.GetMask("Death Walls")))
        {
            // Stops enemy sliding along wall
            _hasHitWall = true;

            // Cancel any physics (may be unnecessary)
            _rb.velocity = Vector2.zero;
            _rb.simulated = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            PhaseController phaseController = GetComponent<PhaseController>();
            
            // FX
            _particleSystem.Play();
            _particleSystem.transform.parent = null;
            
            // Add Time to clock
            FindObjectOfType<PlayerHealth>().AddTime(gameObject, phaseController.GetCurrentPhaseScoreMultiplier());

            // Death
            _isDead = true;
            phaseController.enabled = false;
            // StartCoroutine(DeathDelayTimer());
        }

        /*if (_isDead && other.gameObject.CompareTag("DeathWall"))
        {
            // Stops enemy sliding along wall
            _hasHitWall = true;

            // Cancel any physics (may be unnecessary)
            _rb.velocity = Vector2.zero;
            _rb.simulated = false;

            /// This line was used before enemies impacted with walls.
            /// Still might be useful if performance gets choppy further into game 
            //     // Destroy(gameObject, enemyDeathDelay + 0.1f);
       
            Destroy(gameObject, 15f);
        }
             */
    }
}

/// Idea: On isdead make collider radius large. When collider hits wall
/// make radius zero and slow speed down. Then turn physics off as per current code.