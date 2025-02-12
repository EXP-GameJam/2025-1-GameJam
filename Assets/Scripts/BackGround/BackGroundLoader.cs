using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundLoader : MonoBehaviour
{
    [SerializeField] private Transform _rocket;
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
