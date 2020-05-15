using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TimeBar timeBar;
    
    private float _gameTimer;

    void Start()
    {
        timeBar.SetMaxTime(1);
    }

    void Update()
    {
        
    }
}
