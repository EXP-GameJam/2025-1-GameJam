using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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

    private GameObject _rocket;

    private float lastXPosition;
    
    [SerializeField] private float blankLength = 17f;
    [SerializeField] private float initialSpeed = 1f;
    [SerializeField] private float accelerationWeight;

    private MapGenerator _mapGenerator;

    private void Awake()
    {
        _mapGenerator = this.AddComponent<MapGenerator>();
   
        string prefabPath = "Prefabs/Rocket";
        rocketPrefab = Resources.Load<GameObject>(prefabPath);
        _rocket = Instantiate(rocketPrefab, Vector3.zero, quaternion.identity);
    }

    private void Start()
    {
        if (rocketPrefab != null)
        {
            lastXPosition = _rocket.transform.position.x;
            _mapGenerator.GenerateMap(lastXPosition);
        }
    }

    private void Update()
    {
        gameSpeed = initialSpeed += Time.deltaTime * accelerationWeight;
        elapsedTime += Time.deltaTime;

        if (_rocket != null)
        {
            float currentXPos = _rocket.transform.position.x;

            if (currentXPos - lastXPosition > blankLength)
            {
                lastXPosition = currentXPos;
                _mapGenerator.GenerateMap(lastXPosition);
            } 
        }
    }

    public GameObject GetRocket()
    {
        return _rocket;
    }
}
