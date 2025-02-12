using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Sprite[] obstacleASprite = new Sprite[2];
    [SerializeField] private Sprite[] obstacleBSprite = new Sprite[2];
    [SerializeField] private Sprite[] obstacleCSprite = new Sprite[2];

    private CircleCollider2D circleCollider;
    private SpriteRenderer spriteRenderer;

    private bool bGetScored = false;

    [SerializeField] private float rotateSpeed = 1f;
    
    private float maxY = 4.5f;
    private float minY = -4.5f;

    public int ObstacleSize;
    
    private void Awake()
    {
        // 원형 콜리전 추가 및 설정
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
        
        // 스프라이트 렌더러 추가 및 설정
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void InitializeObstacle(int obstacleNum, int section)
    {
        ObstacleSize = obstacleNum;
        
        if (spriteRenderer != null)
        {
            if (obstacleNum == 1) spriteRenderer.sprite = obstacleASprite[Random.Range(0, 2)];
            if (obstacleNum == 2) spriteRenderer.sprite = obstacleBSprite[Random.Range(0, 2)];
            if (obstacleNum == 3) spriteRenderer.sprite = obstacleCSprite[Random.Range(0, 2)];
        }

        if (circleCollider != null)
        {
            circleCollider.radius = obstacleNum * (maxY - minY) / 13.0f;
        }

        float obstacleY = minY + ((maxY - minY) / 13.0f) * section;

        this.transform.position = new Vector3(this.transform.position.x, obstacleY, 0);
    }

    public void SetSprite(Sprite newSprite)
    {
        GetComponent<SpriteRenderer>().sprite = newSprite;
    }

    private void Update()
    {
        int randomSpeed = 20 + Random.Range(0, 20);

        this.transform.Rotate(0, 0, randomSpeed * rotateSpeed * Time.deltaTime);

        if (!bGetScored)
        {
            GameObject rocket = GameManager.Instance._ingameManager.GetRocket();
            if (rocket != null)
            {
                if (rocket.GetComponent<Transform>().position.x >
                    this.transform.position.x)
                {
                    GameManager.Instance._ingameManager.IncreaseScore();
                    bGetScored = true;
                }
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Rocket"))
        {
            GameManager.Instance._ingameManager.GameEnd();
        }
        Debug.Log("게임 끝!");
    }
}