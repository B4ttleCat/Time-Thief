﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Projectile : MonoBehaviour
{
    private bool _isUsed = false;

    private void OnCollisionEnter2D(Collision2D other)
    {
        // isUsed is an attempt to stop projectile killing two enemies
        // with one shot. Doesn't work!
        if (_isUsed) return;

        _isUsed = true;
        GetComponent<Rigidbody2D>().simulated = false;
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Save the position it hit the target
            Vector2 hitPos = transform.position;
            
            // Parent it to the target
            transform.parent = other.transform;
            
            // Continue to update its position
            transform.position = (Vector3)hitPos - transform.localPosition;
        }
    }
}