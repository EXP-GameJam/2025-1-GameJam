using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private GameObject starlinkPrefab;
    [SerializeField] private float blankStarlinkSpawnChance;

    // 현재 위치 기준으로 얼마나 앞에 장애물을 만들것인가?
    private float frontFront = 40f;
    private LayerMask obstacleLayer;

    [SerializeField] private float earlyPhaseTime = 20f;
    [SerializeField] private float midPhaseTime = 40f;
    [SerializeField] private float lastPhaseTime = 60f;

    [SerializeField] private int ObstacleAWeight = 1;
    [SerializeField] private int ObstacleBWeight = 1;
    [SerializeField] private int ObstacleCWeight = 1;

    private const int TOTAL_SPACES = 12;
    private List<int> availableSpaces;

    private Vector3 lastStarlinkPosition = Vector3.zero;

    private void Awake()
    {
        string prefabPath = "Prefabs/Obstacle";
        obstaclePrefab = Resources.Load<GameObject>(prefabPath);
        prefabPath = "Prefabs/Starlink";
        starlinkPrefab = Resources.Load<GameObject>(prefabPath);
        obstacleLayer = LayerMask.GetMask("Obstacle");
    }

    public void GenerateMap(float xPos)
    {
        Debug.Log("!");
        // TODO
        // x 위치가 주어진다. 어느정도 앞 거리에
        // 1. 장애물 생성
        // 2. 빈공간 생성
        int obstacleCount = GetObstacleCount();

        // 장애물 생성
        int firstObstacleNum, secondObstacleNum;
        do
        {
            firstObstacleNum = GetObstacleByWeight();
            secondObstacleNum = GetObstacleByWeight();
        } while (firstObstacleNum == 3 && secondObstacleNum == 3);

        int firstObstaclePlace, secondObstaclePlace;
        (firstObstaclePlace, secondObstaclePlace) = PlaceObstacles(firstObstacleNum, secondObstacleNum);

        InstantiateObstacle(xPos, firstObstacleNum, firstObstaclePlace, 
            secondObstacleNum, secondObstaclePlace, obstacleCount);
    }

    int GetObstacleCount()
    {
        int obstacleNum = 2;
        // float elapsedTime = GameManager.Instance._ingameManager.elapsedTime;
        // if (elapsedTime < earlyPhaseTime)
        // {
        //     obstacleNum = Random.value < 0.5f ? 1 : 2;
        // }
        // else if (elapsedTime < midPhaseTime)
        // {
        //     obstacleNum = Random.value < 0.2f ? 1 : 2;
        // }
        // else if (elapsedTime < lastPhaseTime)
        // {
        //     obstacleNum = Random.value < 0.05f ? 1 : 2;
        // }
        // else
        // {
        //     obstacleNum = 2;
        // }

        return obstacleNum;
    }
    
    private int GetObstacleByWeight()
    {
        int totalWeight = ObstacleAWeight + ObstacleBWeight + ObstacleCWeight;
        int randomValue = Random.Range(1, totalWeight + 1);
        
        if (randomValue <= ObstacleAWeight)
            return 1; // Obstacle A
        if (randomValue <= ObstacleAWeight + ObstacleBWeight)
            return 2; // Obstacle B
        return 3; // Obstacle C
    }
    
    public (int, int) PlaceObstacles(int firstObstacleSize, int secondObstacleSize)
    {
        availableSpaces = new List<int>();
        for (int i = 1; i <= TOTAL_SPACES; i++) availableSpaces.Add(i); // 초기 공간 설정

        // 첫 번째 장애물 배치
        List<int> firstObstaclePositions = PlaceObstacle(Mathf.Max(firstObstacleSize, secondObstacleSize));
        if (firstObstaclePositions == null) return (-1, -1); // 배치 실패 시 예외 처리

        // 두 번째 장애물 배치
        List<int> secondObstaclePositions = PlaceObstacle(Mathf.Min(firstObstacleSize, secondObstacleSize));
        if (secondObstaclePositions == null) return (-1, -1); // 배치 실패 시 예외 처리

        // 장애물의 중간값 반환
        int firstMiddle = firstObstaclePositions[firstObstaclePositions.Count / 2];
        int secondMiddle = secondObstaclePositions[secondObstaclePositions.Count / 2];

        return (firstMiddle, secondMiddle);
    }
    
    private List<int> PlaceObstacle(int size)
    {
        int requiredSpace = size == 3 ? 5 : (size == 2 ? 3 : 1); // 크기별 필요 공간

        // 배치 가능한 공간 중에서 랜덤 선택
        List<int> validPositions = FindValidPositions(requiredSpace);
        if (validPositions.Count == 0) return null; // 배치할 공간 없음

        int startIndex = validPositions[Random.Range(0, validPositions.Count)];
        List<int> occupiedPositions = new List<int>();

        // 장애물 공간 채우기
        for (int i = 0; i < requiredSpace; i++)
        {
            occupiedPositions.Add(startIndex + i);
            availableSpaces.Remove(startIndex + i);
        }

        // 앞뒤로 1칸 블록 처리
        if (startIndex - 1 >= 1) availableSpaces.Remove(startIndex - 1);
        if (startIndex + requiredSpace <= TOTAL_SPACES) availableSpaces.Remove(startIndex + requiredSpace);

        return occupiedPositions;
    }
    
    private List<int> FindValidPositions(int requiredSpace)
    {
        List<int> validPositions = new List<int>();

        for (int i = 0; i <= availableSpaces.Count - requiredSpace; i++)
        {
            bool canPlace = true;
            for (int j = 0; j < requiredSpace; j++)
            {
                if (!availableSpaces.Contains(availableSpaces[i] + j))
                {
                    canPlace = false;
                    break;
                }
            }
            if (canPlace) validPositions.Add(availableSpaces[i]);
        }

        return validPositions;
    }

    void InstantiateObstacle(float xPos, int firstSize, int firstPos, 
        int secondSize, int secondPos, int obstacleNum)
    {
        Vector3 initPosition = new Vector3(xPos + frontFront, 0, 0);
        GameObject firstObstacle = null, secondObstacle = null; 
        if (obstacleNum == 2)
        {
            int newFirstSize = Mathf.Max(firstSize, secondSize);
            int newSecondSize = Mathf.Min(firstSize, secondSize);
            firstObstacle = Instantiate(obstaclePrefab, initPosition, quaternion.identity);
            firstObstacle.GetComponent<Obstacle>().InitializeObstacle(newFirstSize, firstPos);
            secondObstacle = Instantiate(obstaclePrefab, initPosition, quaternion.identity);
            secondObstacle.GetComponent<Obstacle>().InitializeObstacle(newSecondSize, secondPos);
            ReplaceObstacle(firstObstacle.GetComponent<Obstacle>(), secondObstacle.GetComponent<Obstacle>());

            GameObject starlink = Instantiate(starlinkPrefab, Vector3.zero, quaternion.identity);
            starlink.GetComponent<Starlink>().InitializeStarlink(firstObstacle.transform.position, secondObstacle.transform.position);
            
            if (lastStarlinkPosition != Vector3.zero && Random.Range(0.0f, 1.0f) > 0.5f)
            {
                CreateBlankStarlink(lastStarlinkPosition, starlink.transform.position);
            }
            lastStarlinkPosition = starlink.transform.position;
        }
        else
        {
            firstObstacle = Instantiate(obstaclePrefab, initPosition, quaternion.identity);
            firstObstacle.GetComponent<Obstacle>().InitializeObstacle(firstSize, firstPos);
        }
    }

    void ReplaceObstacle(Obstacle obstacleA, Obstacle obstacleB)
    {
        Vector3 AOriginPosition = obstacleA.transform.position;
        Vector3 BOriginPosition = obstacleB.transform.position;
        
        while (true)
        {
            Vector3 ANewPosition = AOriginPosition;
            Vector3 BNewPosition = BOriginPosition;
            ANewPosition.x += Random.Range(-7.0f, 7.0f);
            BNewPosition.x += Random.Range(-7.0f, 7.0f);
            obstacleA.transform.position = ANewPosition;
            obstacleB.transform.position = BNewPosition;
            
            float obstacleDist = Vector2.Distance(obstacleA.transform.position, obstacleB.transform.position);
            if (obstacleDist > 1.4f * (obstacleA.ObstacleSize + obstacleB.ObstacleSize))
            {
                return;
            }
        }
    }

    void CreateBlankStarlink(Vector3 lastPos, Vector3 nextPos)
    {
        if (Random.Range(0f, 1f) > 0.5f) return;
        
        Debug.Log("2");
        GameObject starlink = Instantiate(starlinkPrefab, Vector3.zero, quaternion.identity);
        Vector3 midPoint = Vector3.Lerp(lastPos, nextPos, Random.Range(0.45f, 0.55f));

        int tryTemp = 0;
        while (true)
        {
            float randY = Random.Range(-4.0f, 4.0f);

            Vector3 newPosition = midPoint;
            newPosition.y = randY;
            
            Collider2D hit = Physics2D.OverlapCircle(newPosition, 4.0f, obstacleLayer);
            if (hit == null)
            {
                starlink.transform.position = newPosition;
                break;
            }
            else
            {
                Debug.Log("뭐에 맞기는 했다!");
            }
            
            tryTemp++;
            if (tryTemp > 50) // 무한 루프 방지 (50번 시도 후 강제 종료)
            {
                Debug.LogWarning("장애물이 많아 위치를 찾지 못했습니다.");
                Destroy(starlink);
                break;
            }
        }
    }
}
