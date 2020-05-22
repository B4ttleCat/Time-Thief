using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Arena : MonoBehaviour
{
    [SerializeField] private Tilemap _arena;

    public Vector2 Size
    {
        get { return _size; }
        private set { _size = value; }
    }
    
    private Vector2 _size;
    
    void Start()
    {
        Size = new Vector2(_arena.size.x, _arena.size.y);
    }
}
