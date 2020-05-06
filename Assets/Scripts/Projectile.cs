using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool _isDestroyed = false; 
    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!_isDestroyed && other.gameObject.CompareTag("Enemy"))
        {
            _isDestroyed = true;
            Destroy(gameObject);
        }
    }
}