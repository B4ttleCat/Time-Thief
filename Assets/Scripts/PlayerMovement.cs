using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Timeline;

public class PlayerMovement : MonoBehaviour
{
    public delegate void PlayerOutOfBounds(float delay);
    public static event PlayerOutOfBounds OnPlayerOutOfBounds;
    public bool IsOutOfBounds { get; private set; }

    [Header("Setup")]
    [SerializeField] private Tilemap _arena;

    [Header("Gameplay")]
    [SerializeField] private float _speed = 10f;

    [SerializeField] private float _playerOffsetX = 0.5f;
    [SerializeField] private float _playerOffsetY = 0.5f;
    [SerializeField] private float _delayTimer = 1f;

    private Rigidbody2D _rb;
    private Vector2 _movement;
    private Camera _cam;
    private SpriteRenderer _playerSprite;
    private MouseInput _mouseInput;
    private Vector2 _arenaSize;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _cam = Camera.main;
        _playerSprite = GetComponentInChildren<SpriteRenderer>();
        _mouseInput = GetComponent<MouseInput>();
    }

    void Update()
    {
        if (GameManager.IsPaused == true) return;

        GetMovementInput();
        CheckSpriteXDirection();
        CheckPlayerPos();
    }

    private void CheckPlayerPos()
    {
        Vector2 playerPos = transform.position;

        if (playerPos.x > _arena.cellBounds.xMax - _playerOffsetX ||
            playerPos.x < _arena.cellBounds.xMin + _playerOffsetX ||
            playerPos.y > _arena.cellBounds.yMax - _playerOffsetY ||
            playerPos.y < _arena.cellBounds.yMin + _playerOffsetY)
        {
            //use event/action here
            if (OnPlayerOutOfBounds !=null)
            {
                OnPlayerOutOfBounds(_delayTimer);
            }
            transform.position = Vector2.zero;
        }
    }

    private void GetMovementInput()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");
    }

    private void CheckSpriteXDirection()
    {
        if (_mouseInput.LookDirection.x > 0f)
        {
            _playerSprite.flipX = false;
        }
        else if (_mouseInput.LookDirection.x < 0f)
        {
            _playerSprite.flipX = true;
        }
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _movement * _speed * Time.deltaTime);
    }
}