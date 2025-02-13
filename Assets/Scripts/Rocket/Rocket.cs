using Unity.VisualScripting;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [Header("Rocket")]
    [SerializeField] private Transform _transform;
    private Rigidbody2D _rocketBody;
    private Animator anim; 
    private bool _canRocketRotate= true;
    private float accel;
    public float Accel => accel;

    [SerializeField] private GameObject explosionPrefab;
    
    // 게임 속도
    [SerializeField] private float _gameSpeed;
    public float GameSpeed => _gameSpeed;
    private float _startTime;
    private bool _isGameStarted = false;

    // 시작 수평 속도
    [SerializeField] private float _startHorizontalSpeed = 1;

    // 수평속도 가중치
    [SerializeField] private float _horizontalSpeedWeight;

    // 시간에 따른 속도비례 가중치
    [SerializeField] private float _speedWeight = 0.01f;
    [SerializeField] private Transform _camera;

    [Header("Input Volume")]
    [SerializeField] private MicrophoneInputAnalyzer _analyzer;

    [SerializeField] private float _deltaRMS;
    [SerializeField] private float _RMSConst;
    [SerializeField] private float _threshold;

    private void Awake()
    {
        _rocketBody = this.GetComponent<Rigidbody2D>();
        if ( _rocketBody == null ) { _rocketBody = this.AddComponent<Rigidbody2D>(); }
    }

    private void Start()
    {
        _analyzer = GameManager.Instance.microphoneInputAnalyzer;
        _transform = this.transform;
        _camera = Camera.main.GetComponent<Transform>();
        anim = GetComponent<Animator>();

        InitRocket();
    }

    public void InitRocket()
    {
        _threshold = _analyzer.GetNoiseVolume();
        _startTime = Time.time;
        _isGameStarted = true;
    }

    void FixedUpdate()
    {
        if ( _isGameStarted )
        {
            _gameSpeed = _startHorizontalSpeed + (Time.time - _startTime) * _speedWeight;

            accel = (_threshold + _deltaRMS) * _gameSpeed;
            float horizontalSpeed = _gameSpeed * _horizontalSpeedWeight;

            _transform.position += Vector3.right * horizontalSpeed * Time.fixedDeltaTime / 5.0f;

            if (_canRocketRotate)
            {
                _rocketBody.AddForce(Vector2.up * accel, ForceMode2D.Force);
                _transform.rotation = Quaternion.Euler
                    (0, 0, Mathf.Atan2(_rocketBody.velocity.y, horizontalSpeed) * Mathf.Rad2Deg);
            }
            else
            {
                _rocketBody.velocity = Vector2.zero;
            }

            _camera.position = new Vector3(_transform.position.x, 0, -10);
        }
    }

    private void Update()
    {
        anim.SetFloat("Loud", _deltaRMS);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        float size = Camera.main.orthographicSize;
        int sign;

        if (other.gameObject.CompareTag("Floor"))
        {
            if (accel <= 9.8)
            {
                sign = -1;
            }
            else
            {
                _canRocketRotate = true;
                return;
            }
        }
        else if (other.gameObject.CompareTag("Ceiling"))
        {
            if (accel >= 9.8)
            {
                //_rocketBody.gravityScale = 0;
                sign = 1;
            }
            else
            {
                _rocketBody.gravityScale = 1;
                _canRocketRotate = true;
                return;
            }
        }
        else
        {
            return;
        }

        _transform.position = new Vector3(_transform.position.x, size * sign - sign * 1.015f, 0);
        _canRocketRotate = false;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            GameManager.Instance._ingameManager.GameEnd();
        }
        else if (other.gameObject.CompareTag("Ceiling"))
        {
            _canRocketRotate = true;
        }
    }

    public void StartExplosion()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        
        Destroy(explosion, 0.5f);
        Destroy(gameObject);
    }

    public void StopRocket()
    {
        _rocketBody.simulated = false;
        enabled = false;
    }

    public void RestartRocket()
    {
        _rocketBody.simulated = true;
        enabled = true;
    }

    public void SetDeltaRMS(float newDeltaRMS)
    {
        _deltaRMS = newDeltaRMS * _RMSConst;
    }

    public float GetDeltaRMS()
    {
        return _deltaRMS;
    }
}
