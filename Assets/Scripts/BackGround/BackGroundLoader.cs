using UnityEngine;

public class BackGroundLoader : MonoBehaviour
{
    [SerializeField] private Transform _rocket;
    [SerializeField] private Transform _backGround1;
    [SerializeField] private Transform _backGround2;
    [SerializeField] private Transform _backGround3;

    private float _backGroundSize;

    private void Awake()
    {
        _backGroundSize = _backGround1.localScale.x * 2 - 3;
    }

    private void Start()
    {
        _rocket = GameManager.Instance._ingameManager.GetRocket().GetComponent<Transform>();
        if (_rocket == null)
        {
            Debug.Log("로켓 제대로 세팅 안됨!");
        }
    }

    void Update()
    {
        if (_rocket != null)
        {
            if (_backGround2.position.x > _backGround1.position.x && _rocket.position.x > _backGround2.position.x + _backGroundSize / 2)
            {
                _backGround1.position = _backGround3.position + Vector3.right * _backGroundSize;
            }
            else if (_backGround3.position.x > _backGround2.position.x && _rocket.position.x > _backGround3.position.x + _backGroundSize / 2)
            {
                _backGround2.position = _backGround1.position + Vector3.right * _backGroundSize;
            }
            else if (_backGround1.position.x > _backGround3.position.x && _rocket.position.x > _backGround1.position.x + _backGroundSize / 2)
            {
                _backGround3.position = _backGround2.position + Vector3.right * _backGroundSize;
            }
        }
    }
}
