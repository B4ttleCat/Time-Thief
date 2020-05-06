using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    private float _currentMovementSpeed;
    private ParticleSystem _particleSystem;
    private Vector3 _targetPos;
    private SpriteRenderer _enemySprite;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _enemySprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        _currentMovementSpeed = phase2MoveSpeed;
    }

    void Update()
    {
        _targetPos = _target.transform.position;
        CheckSpriteXDirection();
        transform.position = Vector2.MoveTowards(transform.position, _targetPos,
            _currentMovementSpeed * Time.deltaTime);
    }
    
    private IEnumerator deathDelayTimer()
    {
        yield return new WaitForSeconds(enemyDeathDelay);
    }

    private void CheckSpriteXDirection()
    {
        Vector2 targetDirection = (_targetPos - transform.position);
        if (targetDirection.x > 0f)
        {
            _enemySprite.flipX = false;
        }
        else if (targetDirection.x < 0f)
        {
            _enemySprite.flipX = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
            _particleSystem.Play();
            _particleSystem.transform.parent = null;
            
            StartCoroutine(deathDelayTimer());

            Destroy(gameObject, enemyDeathDelay + 0.1f);
        }
    }
}