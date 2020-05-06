using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject _target;

    [Header("Gameplay")]
    [SerializeField] private float movementSpeed = 5f;

    private Vector3 _targetPos;

    private void Awake()
    {
    }

    void Update()
    {
        _targetPos = _target.transform.position;
        transform.position = Vector2.MoveTowards(transform.position, _targetPos,
            movementSpeed * Time.deltaTime);
    }
}