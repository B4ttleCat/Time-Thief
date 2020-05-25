using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject _target;

    [Header("Gameplay")]
    public float maxMoveSpeed;

    [SerializeField] private float enemyDeathDelay;
    [SerializeField] private float kickBackForce = 20f;

    [SerializeField] private AudioClip enemyHasBeenShot;

    // Enemy
    private Rigidbody2D _rb;
    private SpriteRenderer _enemySprite;
    private Collider2D _col;
    private bool _canMove;

    // FX
    private ParticleSystem _particleSystem;

    // Target
    private Vector3 _targetPos;
    private Vector2 _targetDirection;

    // Death
    private bool _isDead;
    private bool _hasHitWall;

    private void OnEnable()
    {
        PlayerMovement.OnPlayerOutOfBounds += PauseMovement;
    }

    private void OnDisable()
    {
        PlayerMovement.OnPlayerOutOfBounds -= PauseMovement;
    }

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
        _canMove = true;
    }

    void Update()
    {
        if (!_canMove) return;

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

    // todo check if enemy is in deceased or spawnling phase and turn to static so it can't be bumped
    private void MoveEnemy(float directionMultiplier)
    {
        _targetPos = _target.transform.position;

        // Allows two phases that don't move to have kickback
        if (maxMoveSpeed < 1 && _isDead) maxMoveSpeed = 1;

        transform.position = Vector2.MoveTowards(transform.position, _targetPos,
            maxMoveSpeed * directionMultiplier * Time.deltaTime);
    }

    private IEnumerator DelayTimer(float delay)
    {
        _canMove = false;
        yield return new WaitForSecondsRealtime(delay);
        _canMove = true;
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

    private void PauseMovement(float delay)
    {
        StartCoroutine(DelayTimer(delay));

        // todo instead of stopping enemy movement, perhaps make time slow down to help player reposition better
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

            // Physics
            _rb.velocity = Vector2.zero; // Don't contribute to knockback force variable

            // Sprite swap
            phaseController.UseDeathSprite();

            // FX
            _particleSystem.Play();
            // _particleSystem.transform.parent = null;

            // Audio
            PlayAudio();

            // Add Time to clock
            FindObjectOfType<PlayerHealth>().AddTime(gameObject, phaseController.GetCurrentPhaseScoreMultiplier());

            // Death
            _isDead = true;
            phaseController.enabled = false;
            // StartCoroutine(DeathDelayTimer());
        }
    }

    private void PlayAudio()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = enemyHasBeenShot;
        float randomPitch = Random.Range(0.95f, 1.05f);
        audioSource.pitch = randomPitch;
        audioSource.Play();
    }
}