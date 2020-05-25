using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerShooting : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private AudioClip playerFired;
    
    [Header("Gameplay")]
    [SerializeField] private float projectileForce = 40f;
    [SerializeField] private float fireRate = 1f;

    private Rigidbody2D _rb;
    private MouseInput _mouseInput;
    private AudioSource _audioSource;
    private float _nextFire = 0f;
    
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
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
        _nextFire = Time.time + fireRate;
        
        // Orient the spawner to the face the mouse cursor
        projectileSpawnPoint.rotation = quaternion.Euler(0, 0, _mouseInput.Angle);
        
        // Spawn the bullet
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position,quaternion.Euler(0,0, _mouseInput.Angle));

        // Add force to projectile
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectileRb.AddForce(projectileSpawnPoint.right * projectileForce, ForceMode2D.Impulse);
        
        // SFX
        PlayAudio();
    }

    private void PlayAudio()
    {
        _audioSource.clip = playerFired;
        float randomPitch = Random.Range(0.95f, 1.05f);
        _audioSource.pitch = randomPitch;
        _audioSource.Play();
    }
}