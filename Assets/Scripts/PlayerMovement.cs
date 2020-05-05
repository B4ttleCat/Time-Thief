using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;
    private Rigidbody2D _rb;
    private Vector2 _movement;
    private Camera _cam;
    private Vector2 _mousePos;

    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        _mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate ()
    {
        _rb.MovePosition(_rb.position + _movement * _speed * Time.deltaTime);
    }
}