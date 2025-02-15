using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CorrectionUI : MonoBehaviour
{
    [SerializeField] private GameObject _correctionPanel;
    [SerializeField] private GameObject[] _countDownElements;
    [SerializeField] private GameObject[] _loadingElements;

    [SerializeField] private TextMeshProUGUI _countDownText;
    [SerializeField] private Transform _loadingGauge;

    private GameObject[] _gaugeBlocks;

    private float _sensitivity;

    public delegate void CorrectionEndEvent();

    private void Awake()
    {
        _gaugeBlocks = new GameObject[_loadingGauge.childCount];

        for (int i = 0; i < _loadingGauge.childCount; i++)
        {
            _gaugeBlocks[i] = _loadingGauge.GetChild(i).gameObject;
        }
    }

    /// <summary> ���� ���� </summary>
    public async void StartCorrection(CorrectionEndEvent endEvent)
    {
        _correctionPanel.SetActive(true);

        foreach (GameObject element in _countDownElements)
        {
            element.SetActive(true);

        }
        foreach (GameObject element in _loadingElements)
        {
            element.SetActive(false);
        }

        for (int i = 5; i > 0; i--)
        {
            _countDownText.text = i.ToString();
            await Task.Delay(1000);
        }

        foreach (GameObject element in _countDownElements)
        {
            element.SetActive(false);

        }

        foreach (GameObject go in _gaugeBlocks)
        {
            go.SetActive(false);
        }

        foreach (GameObject element in _loadingElements)
        {
            element.SetActive(true);
        }

        // ���� ����
        StartCoroutine(CheckNoise(5f));

        int gaugeCnt = _gaugeBlocks.Length;

        for (int i = 0; i < gaugeCnt; i++)
        {
            await Task.Delay(5500 / (gaugeCnt + 1));
            _gaugeBlocks[i].SetActive(true);
        }
        await Task.Delay(5500 / (gaugeCnt + 1) + 5500 % (gaugeCnt + 1));

        endEvent?.Invoke();
        _correctionPanel.SetActive(false);
    }
    
    IEnumerator CheckNoise(float duration)
    {
        float curTime = 0f;
        float sumVolume = 0f;
        int sumCount = 0;

        MicrophoneInputAnalyzer microphoneAnalyzer = GameManager.Instance.microphoneInputAnalyzer;
        if (microphoneAnalyzer == null)
        {
            Debug.Log("microphoneAnalyzer ���� �ȵ�!");
            yield break;
        }
        
        while (curTime < duration)
        {
            curTime += Time.unscaledDeltaTime;
            sumCount++;
            sumVolume += microphoneAnalyzer.currentVolume;
            
            yield return null;
        }
        
        // ���� ���⿡ �÷��̾ �������� ������ �� �߰����ش�
        float newNoiseVolume = (sumVolume / sumCount) + 0.04f + _sensitivity;
        microphoneAnalyzer.SetNoiseVolume(newNoiseVolume);
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene");
    }

    public void SetSensitivity(float amount)
    {
        _sensitivity = amount / 50;
    }
}
