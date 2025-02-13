using System;
using UnityEngine;

public class MicrophoneInputAnalyzer : MonoBehaviour
{
    public string microphoneDevice;

    private AudioClip micClip;
    private int sampleRate = 44100;
    private int sampleSize = 1024;
    
    private float[] audioSamples = new float[1024];  
    
    private float baseVolume = 0f;
    private float noiseVolume = 0f; 
    
    public bool hasBaseVolume = false;
    public bool hasNoiseVolume = false;
    
    public float currentVolume = 0f;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("SavedNoise"))
        {
            hasNoiseVolume = true;
            noiseVolume = PlayerPrefs.GetFloat("SavedNoise");
        }
        
    }

    void Start()
    {
        // 마이크 장치 가져오기
        if (Microphone.devices.Length > 0)
        {
            microphoneDevice = Microphone.devices[0];
            StartMicrophone();
        }
        else
        {
            Debug.LogError("마이크를 찾을 수 없습니다.");
        }
    }

    void StartMicrophone()
    {
        micClip = Microphone.Start(microphoneDevice, true, 1, sampleRate);
        audioSamples = new float[sampleSize];
    }

    void Update()
    {
        if (Microphone.IsRecording(microphoneDevice))
        {
            if (GetVolume() > 1.5f) return;
            currentVolume = GetVolume();
        }
    }

    float GetVolume()
    {
        if (micClip == null) return 0f;

        int micPosition = Microphone.GetPosition(microphoneDevice);
        if (micPosition < sampleSize) return 0f;

        micClip.GetData(audioSamples, micPosition - sampleSize);

        float sum = 0f;
        foreach (float sample in audioSamples)
        {
            sum += sample * sample;
        }

        return Mathf.Sqrt(sum / audioSamples.Length);
    }
    
    // Getter & Setter
    public void SetNoiseVolume(float newNoiseVolume)
    {
        noiseVolume = newNoiseVolume;
        hasNoiseVolume = true;
        PlayerPrefs.SetFloat("SavedNoise", newNoiseVolume);
        Debug.Log($"NoiseVolume set to : {noiseVolume}");
    }

    public float GetNoiseVolume()
    {
        return noiseVolume; 
    }

    public void SetBaseVolume(float newBaseVolume)
    {
        baseVolume = newBaseVolume;
        hasBaseVolume = true;
        Debug.Log($"BaseVolume set to : {baseVolume}");
    }
}