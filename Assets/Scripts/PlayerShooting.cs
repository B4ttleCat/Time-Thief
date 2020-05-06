using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject projectilePrefab;
    
    [Header("Gameplay")]
    [SerializeField] private float projectileForce = 10f;
    [SerializeField] private float _fireRate = 1f;

    private Rigidbody2D _rb;
    private MouseInput _mouseInput;

    private float _nextFire = 0f;
    
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _mouseInput = GetComponent<MouseInput>();
    }

    void Update()
    {
        if (Input.GetButtonUp("Fire1") && Time.time > _nextFire)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        // Update the fire rate timer
        _nextFire = Time.time + _fireRate;
        
        // Orient the spawner to the face the mouse cursor
        projectileSpawnPoint.rotation = quaternion.Euler(0, 0, _mouseInput.Angle);
        
        // Spawn the bullet
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position,quaternion.Euler(0,0, _mouseInput.Angle));

        // Add force to projectile
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.AddForce(projectileSpawnPoint.right * projectileForce, ForceMode2D.Impulse);
    }
}