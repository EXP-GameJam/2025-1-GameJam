using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public MicrophoneInputAnalyzer microphoneInputAnalyzer;
    public IngameManager _ingameManager;

    // Singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject singletonObject = new GameObject(typeof(GameManager).Name);
                instance = singletonObject.AddComponent<GameManager>();
                DontDestroyOnLoad(singletonObject);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        microphoneInputAnalyzer = this.AddComponent<MicrophoneInputAnalyzer>();
        _ingameManager = this.AddComponent<IngameManager>();
    }
}

