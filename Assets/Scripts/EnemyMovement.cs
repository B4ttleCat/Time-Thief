using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyMovement : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject _target;

    [Header("Gameplay")]
    [SerializeField] private float phase1MoveSpeed;
    [SerializeField] private float phase2MoveSpeed;
    [SerializeField] private float phase3MoveSpeed;
    [SerializeField] private float phase4MoveSpeed;
    
    [SerializeField] private float enemyDeathDelay;
    [SerializeField] private float kickBackForce = 20f;

    private Rigidbody2D _rb;
    private float _currentMaxMoveSpeed;
    private ParticleSystem _particleSystem;
    private Vector3 _targetPos;
    private SpriteRenderer _enemySprite;
    private bool _isDead;
    private bool _hasHitWall;
    private Vector2 _targetDirection;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _particleSystem = GetComponent<ParticleSystem>();
        _enemySprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        _currentMaxMoveSpeed = phase2MoveSpeed;
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
        }
    }

    private void MoveEnemy(float directionMultiplier)
    {
        _targetPos = _target.transform.position;

        transform.position = Vector2.MoveTowards(transform.position, _targetPos,
            _currentMaxMoveSpeed * directionMultiplier * Time.deltaTime);
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            // FX
            _particleSystem.Play();
            _particleSystem.transform.parent = null;
            
            // Death
            _isDead = true;
            // StartCoroutine(DeathDelayTimer());
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        
        if (_isDead && other.gameObject.CompareTag("DeathWall"))
        {
            // Stops enemy sliding along wall
            _hasHitWall = true;
            
            // Cancel any physics (may be unnecessary)
            _rb.velocity = Vector2.zero;
            _rb.simulated = false;

            // _rb.bodyType = RigidbodyType2D.Static;

            // _rb.isKinematic = true;
            //     // Destroy(gameObject, enemyDeathDelay + 0.1f);
        }
    }
}