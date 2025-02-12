using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButtonController : MonoBehaviour
{
    public Button checkNoiseButton;
    public Button speakButton;
    public Button checkSpeechButton;

    [SerializeField] private MicrophoneInputAnalyzer microphoneAnalyzer;
    
    public void Awake()
    {
        checkNoiseButton.onClick.AddListener(OnCheckNoiseClicked);
        speakButton.onClick.AddListener(OnSpeakButtonClicked);
        checkSpeechButton.onClick.AddListener(OnCheckSpeechButtonClicked);
    }

    private void Start()
    {
        microphoneAnalyzer = GameManager.Instance.microphoneInputAnalyzer;
    }

    private void OnCheckNoiseClicked()
    {
        checkNoiseButton.interactable = false;
        StartCoroutine(CheckNoise(5f));
    }
    
    IEnumerator CheckNoise(float duration)
    {
        float curTime = 0f;
        float sumVolume = 0f;
        int sumCount = 0;

        if (microphoneAnalyzer == null)
        {
            Debug.Log("microphoneAnalyzer 세팅 안됨!");
            yield break;
        }
        
        while (curTime < duration)
        {
            curTime += Time.deltaTime;
            sumCount++;
            sumVolume += microphoneAnalyzer.currentVolume;
            
            yield return null;
        }
        
        // 이후 여기에 플레이어가 설정에서 세팅한 값 추가해준다
        float newNoiseVolume = (sumVolume / sumCount) + 0.04f;
        microphoneAnalyzer.SetNoiseVolume(newNoiseVolume);
        checkNoiseButton.interactable = true;
    }
    
    private void OnCheckSpeechButtonClicked()
    {
        checkSpeechButton.interactable = false;
        StartCoroutine(CheckVolume(2f));
    }
    
    IEnumerator CheckVolume(float duration)
    {
        float curTime = 0f;
        float sumVolume = 0f;
        int sumCount = 0;
        
        if (microphoneAnalyzer == null)
        {
            Debug.Log("microphoneAnalyzer 세팅 안됨!");
            yield break;
        }
        
        while (curTime < duration)
        {
            curTime += Time.deltaTime;
            
            if (microphoneAnalyzer.currentVolume > microphoneAnalyzer.GetNoiseVolume())
            {
                sumCount++;
                sumVolume += microphoneAnalyzer.currentVolume;
            }
            
            yield return null;
        }
        
        // 이후 여기에 플레이어가 설정에서 세팅한 값 추가해준다
        microphoneAnalyzer.SetBaseVolume(sumVolume / sumCount);

        checkSpeechButton.interactable = true;
    }
    
    
    private void OnSpeakButtonClicked()
    {
        
    }
}
