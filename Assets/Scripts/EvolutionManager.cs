using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject[] phasePrefabs;

    [Header("Gameplay")]
    [SerializeField] private const float TimeUntilLastPhase = 15f;

    
    private GameObject _currentPhasePrefab;
    private float _timer;
    private Sprite _sprite;
    private int _nextPhaseIndex = 0;
    private bool _hasReachedFinalEvolution;

    void Start()
    {
        _currentPhasePrefab = UpdateCurrentPhasePrefab(phasePrefabs[_nextPhaseIndex]);
    }

    private GameObject UpdateCurrentPhasePrefab(GameObject nextPrefab)
    {
        _nextPhaseIndex++;

        if (_nextPhaseIndex >= phasePrefabs.Length)
        {
            _hasReachedFinalEvolution = true;
            return _currentPhasePrefab;
        }

        return nextPrefab;
    }

    void Update()
    {
        if (_hasReachedFinalEvolution) return;

        _timer += Time.deltaTime;

        // todo rig the 1f to the timer on each prefab 
        if (_timer >= 1f)
        {
            _timer = 0f;
            _currentPhasePrefab = UpdateCurrentPhasePrefab(phasePrefabs[_nextPhaseIndex]);
        }
    }
}