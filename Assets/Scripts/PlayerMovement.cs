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
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        _mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate ()
    {
        _rb.MovePosition(_rb.position + _movement * _speed * Time.deltaTime);

        // Vector2 lookDirection = _mousePos - _rb.position;
        //
        // // consider adding "- 90f" to the end here to fix potential rotational issues
        // float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        // _rb.rotation = angle;
    }
}