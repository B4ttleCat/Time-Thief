using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;

    private Rigidbody2D _rb;
    private Vector2 _movement;
    private Camera _cam;
    private SpriteRenderer _playerSprite;
    private MouseInput _mouseInput;

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