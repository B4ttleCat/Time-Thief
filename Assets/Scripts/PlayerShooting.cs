using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileForce = 20f;

    private Vector2 _mousePos;
    private Rigidbody2D _rb;
    private Camera _cam;

    void Awake()
    {
        _cam = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
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
        
        GameObject projectile = Instantiate(projectilePrefab, 
                                            projectileSpawnPoint.position, 
                                            quaternion.identity);
        
        // projectile.transform.LookAt(_mousePos);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.AddForce(projectileSpawnPoint.right * projectileForce, ForceMode2D.Impulse);
    }
}