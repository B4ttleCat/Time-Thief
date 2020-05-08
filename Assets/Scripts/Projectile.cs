using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Debug.Log("used");
            transform.parent = other.transform;
            transform.position = transform.parent.localPosition;
        }
    }
}