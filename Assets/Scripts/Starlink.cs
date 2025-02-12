using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Starlink : MonoBehaviour
{
    private CircleCollider2D circleCollider;

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    public void InitializeStarlink(Vector3 fVec, Vector2 sVec)
    {
        Vector3 midPoint = Vector3.Lerp(fVec, new Vector3(sVec.x, sVec.y, fVec.z), Random.Range(0.4f, 0.6f));
      
        Vector2 direction = new Vector2(sVec.x - fVec.x, sVec.y - fVec.y).normalized;
   
        Vector2 perpendicular = new Vector2(-direction.y, direction.x); // 기존 벡터를 시계 방향으로 90도 회전

        Vector2 randomPoint2D;
        do
        {
            float randomOffset = Random.Range(-3f, 3f);
            randomPoint2D = midPoint + (Vector3)(perpendicular * randomOffset);
        } while (randomPoint2D.y > 4.0f || randomPoint2D.y < -4.0f);
        
        transform.position = new Vector3(randomPoint2D.x, randomPoint2D.y, 0f);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Rocket"))
        {
            GameManager.Instance._ingameManager.IncreaseScore();
            Destroy(gameObject);
        }
    }
}
