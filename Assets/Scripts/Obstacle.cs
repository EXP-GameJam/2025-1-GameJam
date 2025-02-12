using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Sprite[] obstacleSprite = new Sprite[3];

    private CircleCollider2D circleCollider;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        // 원형 콜리전 추가 및 설정
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
        
        // 스프라이트 렌더러 추가 및 설정
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void InitializeObstacle(int obstacleNum, int section)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = obstacleSprite[obstacleNum - 1];
        }

        if (circleCollider != null)
        {
            circleCollider.radius = obstacleNum * 3f;
        }
    }

    public void SetSprite(Sprite newSprite)
    {
        GetComponent<SpriteRenderer>().sprite = newSprite;
    }
}