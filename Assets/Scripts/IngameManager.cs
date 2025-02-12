using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IngameManager : MonoBehaviour
{
    public int score;
    public float gameSpeed;
    public float progressDistance;
    public float elapsedTime;
    
    [SerializeField]
    private GameObject rocketPrefab;

    private GameObject startPosition;
    private GameObject _rocket;

    private float lastXPosition;
    
    [SerializeField] private float blankLength;
    [SerializeField] private float initialSpeed = 1f;
    [SerializeField] private float accelerationWeight;

    private MapGenerator _mapGenerator;

    private void Awake()
    {
        _mapGenerator = this.AddComponent<MapGenerator>();
    }

    private void Start()
    {
        if (rocketPrefab != null)
        {
            _rocket = Instantiate(rocketPrefab, startPosition.transform.position, Quaternion.identity);
        }
        lastXPosition = _rocket.transform.position.x;
    }

    private void Update()
    {
        gameSpeed = initialSpeed += Time.deltaTime * accelerationWeight;
        elapsedTime += Time.deltaTime;
        
        float currentXPos = _rocket.transform.position.x;

        if (currentXPos - lastXPosition > blankLength)
        {
            _mapGenerator.GenerateMap(lastXPosition);
            lastXPosition = currentXPos;
        }
    }
}
