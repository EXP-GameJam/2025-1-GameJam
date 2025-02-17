using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup _bgmMixer;
    [SerializeField] private AudioMixerGroup _sfxMixer;

    [SerializeField] private AudioClip _titleClip;
    [SerializeField] private AudioClip _gameClip;
    [SerializeField] private AudioClip _gameOverClip;

    [SerializeField] private AudioClip _sqareButtonClip;
    [SerializeField] private AudioClip _smallButtonClip;
    [SerializeField] private AudioClip _volumeClip;
    [SerializeField] private AudioClip _fallClip;
    [SerializeField] private AudioClip _crashClip;

    private AudioSource _bgmSource;
    private Stack<AudioSource> _sfxStack = new Stack<AudioSource>();
    private List<AudioSource> _playingSFX = new List<AudioSource>();

    private int maxSFXSources = 5;

    private static SoundManager _instance;
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject singletonObject = new GameObject(typeof(SoundManager).Name);
                _instance = singletonObject.AddComponent<SoundManager>();
                DontDestroyOnLoad(singletonObject);
            }
            return _instance;
        }
    }


    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _bgmSource = this.AddComponent<AudioSource>();
        _bgmSource.loop = true;
        _bgmSource.playOnAwake = true;
        _bgmSource.outputAudioMixerGroup = _bgmMixer;

        for (int i = 0; i < 2; i++)
        {

            _sfxStack.Push(AddNewAudioSource());
        }

        SceneManager.sceneLoaded += ChangeBGM;

    }

    private AudioSource AddNewAudioSource()
    {
        AudioSource audioSource = this.AddComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.playOnAwake = false;
        audioSource.outputAudioMixerGroup = _sfxMixer;

        return audioSource;
    }

    private void ChangeBGM(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;
        if (sceneName == "TitleScene")
        {
            _bgmSource.clip = _titleClip;
        }
        else if (sceneName == "GameScene")
        {
            _bgmSource.clip = _gameClip;
        }
        _bgmSource.Play();
    }

    public void PlayGameOverBGM()
    {
        _bgmSource.Stop();
        _bgmSource.clip = _gameOverClip;
        _bgmSource.Play();
    }

    private void PlaySFX(AudioClip clip)
    {
        AudioSource audioSource;
        if (!_sfxStack.TryPop(out audioSource))
            audioSource = AddNewAudioSource();

        audioSource.clip = clip;
        audioSource.Play();
        _playingSFX.Add(audioSource);
    }

    public void PlaySquareButtonSound()
    {
        PlaySFX(_sqareButtonClip);
    }

    public void PlaySmallButtonSound()
    {
        PlaySFX(_smallButtonClip);
    }

    public void PlayVolumeSound(bool isForce = false)
    {
        if (isForce)
        {
            PlaySFX(_volumeClip);
        }
        else if (_playingSFX.Count == 0)
            PlaySFX(_volumeClip);
    }

    public void PlayFallSound()
    {
        PlaySFX(_fallClip);
    }

    public void PlayCrashSound()
    {
        PlaySFX(_crashClip);
    }

    private void Update()
    {
        for (int i = _playingSFX.Count - 1; i >= 0 ; i--)
        {
            if (!_playingSFX[i].isPlaying)
            {
                if (_sfxStack.Count < maxSFXSources)
                {
                    _sfxStack.Push(_playingSFX[i]);
                }
                else
                {
                    Destroy(_playingSFX[i]);
                }
                _playingSFX.RemoveAt(i);
            }
        }
    }
}
