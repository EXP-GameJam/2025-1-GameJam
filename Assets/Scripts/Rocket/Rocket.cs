using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private Transform _rocket;
    private Rigidbody2D _rocketBody;

    // 게임 속도
    [SerializeField] private float _gameSpeed;

    // 시작 수평 속도
    [SerializeField] private float _startHorizontalSpeed = 1;

    // 수평속도 가중치
    [SerializeField] private float _horizontalSpeedWeight;

    // 시간에 따른 속도비례 가중치
    [SerializeField] private float _speedWeight = 0.01f;
    [SerializeField] private Transform _camera;


    [SerializeField] float _deltaRMS;
    [SerializeField] private float _RMSConst;
    [SerializeField] private float _threshold;

    private void Awake()
    {
        _rocketBody = _rocket.GetComponent<Rigidbody2D>();
        if ( _rocketBody == null ) { _rocketBody = _rocket.AddComponent<Rigidbody2D>(); }
    }

    void FixedUpdate()
    {
        _gameSpeed = _startHorizontalSpeed + Time.time * _speedWeight;

        float accel = (_threshold + _RMSConst * _deltaRMS) * _gameSpeed;
        float horizontalSpeed = _gameSpeed * _horizontalSpeedWeight;

        _rocket.position += Vector3.right * horizontalSpeed * Time.fixedDeltaTime;
        _rocketBody.AddForce(Vector2.up * accel, ForceMode2D.Force);
        _rocket.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(_rocketBody.velocity.y, horizontalSpeed) * Mathf.Rad2Deg));
        _camera.position = _rocket.position + Vector3.back * 10;
    }

    public void SetRMS(float volume)
    {
        _deltaRMS = volume;
    }
}
