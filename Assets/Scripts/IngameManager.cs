using System;
using System.Threading.Tasks;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class IngameManager : MonoBehaviour
{
    public int score;
    public float gameSpeed;
    public float progressDistance;
    public float elapsedTime;
    
    [SerializeField]
    private GameObject rocketPrefab;
    
    private GameObject _rocket;

    // Camera Shake
    public Camera mainCamera;
    private Vector3 cameraPos;
    private float duration;
    private float shakeRange;

    private float lastXPosition;
    
    [SerializeField] private float blankLength = 17f;
    [SerializeField] private float initialSpeed = 1f;
    [SerializeField] private float accelerationWeight;

    private MapGenerator _mapGenerator;

    // Delegate
    public event Action<int> OnScoreChanged;
    public event Action<int> OnGameEnd;
    
    private void Init()
    {
        string prefabPath = "Prefabs/Rocket";
        rocketPrefab = Resources.Load<GameObject>(prefabPath);
        _rocket = Instantiate(rocketPrefab, Vector3.zero, quaternion.identity);
        
        if (rocketPrefab != null)
        {
            lastXPosition = _rocket.transform.position.x;
            _mapGenerator.GenerateMap(lastXPosition);
        }
        mainCamera = Camera.main.GetComponent<Camera>();
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        _mapGenerator = this.AddComponent<MapGenerator>();
    }

    private void Update()
    {
        gameSpeed = initialSpeed += Time.deltaTime * accelerationWeight;
        elapsedTime += Time.deltaTime;

        if (_rocket != null)
        {
            float currentXPos = _rocket.transform.position.x;

            if (currentXPos - lastXPosition > blankLength)
            {
                lastXPosition = currentXPos;
                _mapGenerator.GenerateMap(lastXPosition);
            } 
        }
        
        InputVolume();
    }

    public void GameStart()
    {
        elapsedTime = 0f;
        score = 0;
    }

    public async void GameEnd()
    {
        _rocket.GetComponent<Rocket>().StopRocket();
        _rocket.GetComponent<Rocket>().StartExplosion();
        Shake();

        await Task.Delay(1500);
        OnGameEnd?.Invoke(score);
    }

    public void Shake()
    {
        shakeRange = 0.05f;
        duration = 1f;
        
        cameraPos = mainCamera.transform.position;
        InvokeRepeating("StartShake", 0f, 0.005f);
        Invoke("StopShake", duration);
    }

    void StartShake()
    {
        float cameraPosX = Random.value * shakeRange * 2 - shakeRange;
        float cameraPosY = Random.value * shakeRange * 2 - shakeRange;
        Vector3 tcameraPos = mainCamera.transform.position;
        tcameraPos.x += cameraPosX;
        tcameraPos.y = cameraPosY;
        mainCamera.transform.position = tcameraPos;
    }

    void StopShake()
    {
        CancelInvoke("StartShake");
        mainCamera.transform.position = cameraPos;
    }
    
    public GameObject GetRocket()
    {
        return _rocket;
    }

    public void IncreaseScore()
    {
        score++;
        OnScoreChanged?.Invoke(score);
    }

    private void InputVolume()
    {
        float CurrentVolume = GameManager.Instance.microphoneInputAnalyzer.currentVolume;
        float NoiseVolume = GameManager.Instance.microphoneInputAnalyzer.GetNoiseVolume();
        float RelativeVolume = 0f;
        if (CurrentVolume > NoiseVolume)
        {
            RelativeVolume = CurrentVolume - NoiseVolume;
        }
        if (GameManager.Instance.microphoneInputAnalyzer.hasNoiseVolume == true)
        {
            if (_rocket != null)
            {
                _rocket.GetComponent<Rocket>().SetDeltaRMS(RelativeVolume);
                float NextVolume = Mathf.Max(-10f, _rocket.GetComponent<Rocket>().GetDeltaRMS() - 15 * Time.deltaTime);
            }
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            Init();
        }
    }
}
