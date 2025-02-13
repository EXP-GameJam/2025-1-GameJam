using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SliderHandler : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _sensitivitySlider;

    [SerializeField] private CorrectionUI _correctionUI;


    // Start is called before the first frame update
    void Start()
    {
        _bgmSlider.onValueChanged.AddListener(ChangeBgm);
        _sfxSlider.onValueChanged.AddListener(ChangeSfx);
        _sensitivitySlider.onValueChanged.AddListener(ChangeSensitivity);

        InitSlider();
    }

    private void InitSlider()
    {
        _bgmSlider.value = PlayerPrefs.GetFloat("BGM", 1);
        _sfxSlider.value = PlayerPrefs.GetFloat("SFX", 1);
        _sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", 1);
    }

    private void ChangeBgm(float value)
    {
        _audioMixer.SetFloat("BGM", GetVolume(value));
        PlayerPrefs.SetFloat("BGM", value);
        PlayerPrefs.Save();
    }

    private void ChangeSfx(float value)
    {
        _audioMixer.SetFloat("SFX", GetVolume(value));
        PlayerPrefs.SetFloat("SFX", value);
        PlayerPrefs.Save();
    }

    private void ChangeSensitivity(float value)
    {
        _audioMixer.SetFloat("Sensitivity", value);
        PlayerPrefs.SetFloat("Sensitivity", value);
        PlayerPrefs.Save();

        _correctionUI.SetSensitivity(value);
    }

    private float GetVolume(float value) => Mathf.Log10(value) * 20;

    [ContextMenu("Reset Prefs")]
    public void ResetPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
