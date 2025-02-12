using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MicrophoneInputAnalyzer microphoneInputAnalyzer;
    //public IngameManager ingameManager;
    
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
        }
        
        microphoneInputAnalyzer = this.AddComponent<MicrophoneInputAnalyzer>();
        //ingameManager = this.AddComponent<IngameManager>();
    }
}

