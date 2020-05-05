using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileForce = 10f;

    private Rigidbody2D _rb;
    private MouseInput _mouseInput;
    
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _mouseInput = GetComponent<MouseInput>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        // call new MouseInput script here
        
        projectileSpawnPoint.rotation = quaternion.Euler(0, 0, _mouseInput.Angle);

        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position,quaternion.Euler(0,0, _mouseInput.Angle));

        // Add force to projectile
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.AddForce(projectileSpawnPoint.right * projectileForce, ForceMode2D.Impulse);
    }
}