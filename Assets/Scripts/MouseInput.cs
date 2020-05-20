using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    private Camera _cam;
    private Vector2 _mousePos;
    private Rigidbody2D _rb;
    public float Angle { get; private set; }
    public Vector2 LookDirection { get; private set; }

    void Awake()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _cam = Camera.main;
    }

    void Update()
    {
        GetMouseCursorPosition();
    }

    private void GetMouseCursorPosition()
    {
        // Convert mouse pos to usable format
        _mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);

        // Get vector for mouse cursor
        LookDirection = _mousePos - _rb.position;

        // Get angle of vector to cursor from player
        Angle = Mathf.Atan2(LookDirection.y, LookDirection.x);
    }
}