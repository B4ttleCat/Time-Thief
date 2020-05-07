using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool _isUsed = false; 
    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Attempt to stop projectile killing two enemies
        // Doesn't work!
        if (_isUsed) return;
        
        if (other.gameObject.CompareTag("Enemy"))
        {
            _isUsed = true;
            Destroy(gameObject);
        }
    }
}