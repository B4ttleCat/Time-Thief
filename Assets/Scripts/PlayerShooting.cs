using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileForce = 10f;

    private Vector2 _mousePos;
    private Rigidbody2D _rb;
    private Camera _cam;

    void Awake()
    {
        _cam = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);


        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Vector2 lookDirection = _mousePos - _rb.position;
        Debug.DrawLine(transform.position, _mousePos, Color.magenta, 0.5f);
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x);
        projectileSpawnPoint.rotation = quaternion.Euler(0, 0, angle);
        
        GameObject projectile = Instantiate(projectilePrefab,
            projectileSpawnPoint.position,
            quaternion.Euler(0,0, angle));

        // Add force to projectile
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.AddForce(projectileSpawnPoint.right * projectileForce, ForceMode2D.Impulse);
    }
}