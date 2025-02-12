using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private float blankStarlinkSpawnChance;

    private float frontFront = 100f;

    [SerializeField] private float earlyPhaseTime = 20f;
    [SerializeField] private float midPhaseTime = 40f;
    [SerializeField] private float lastPhaseTime = 60f;

    [SerializeField] private int ObstacleAWeight = 1;
    [SerializeField] private int ObstacleBWeight = 2;
    [SerializeField] private int ObstacleCWeight = 3;
    
    public void GenerateMap(float xPos)
    {
        // TODO
        // x 위치가 주어진다. 어느정도 앞 거리에
        // 1. 장애물 생성
        // 2. 빈공간 생성
        int obstacleNum = GetObstacleNum();

        // 장애물 생성
        
    }

    int GetObstacleNum()
    {
        int obstacleNum = 1;
        //float elapsedTime = GameManager.Instance.ingameManager.elapsedTime;
        float elapsedTime = 1f;
        if (elapsedTime < earlyPhaseTime)
        {
            obstacleNum = Random.value < 0.5f ? 1 : 2;
        }
        else if (elapsedTime < midPhaseTime)
        {
            obstacleNum = Random.value < 0.2f ? 1 : 2;
        }
        else if (elapsedTime < lastPhaseTime)
        {
            obstacleNum = Random.value < 0.05f ? 1 : 2;
        }
        else
        {
            obstacleNum = 2;
        }

        return obstacleNum;
    }
}
