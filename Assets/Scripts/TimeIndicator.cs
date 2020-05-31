using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeIndicator : MonoBehaviour
{
    [SerializeField] private float speed; 
    [SerializeField] private float lifeTime;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
    }
}
