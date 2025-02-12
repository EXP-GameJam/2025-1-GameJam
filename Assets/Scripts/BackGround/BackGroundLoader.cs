using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundLoader : MonoBehaviour
{
    [SerializeField] private Transform _rocket;

    // 두 배경의 크기는 같아야 함 => 아님 하나만 놓고 복제해서 쓰는법도 괜찮을듯
    [SerializeField] private Transform _backGround1;
    [SerializeField] private Transform _backGround2;

    private float _backGroundSize;

    private void Awake()
    {
        _backGroundSize = _backGround1.localScale.x;
    }

    void Update()
    {
        if (_backGround1.position.x > _backGround2.position.x && _rocket.position.x > _backGround1.position.x)
        {
            _backGround2.position = _backGround1.position + Vector3.right * _backGroundSize;
        }
        else if (_backGround2.position.x > _backGround1.position.x && _rocket.position.x > _backGround2.position.x)
        {
            _backGround1.position = _backGround2.position + Vector3.right * _backGroundSize;
        }
    }
}
