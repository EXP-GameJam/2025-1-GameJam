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

    /// <summary> 보정 시작 </summary>
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

        // 보정
        // 보정완료한거 GameManger에게 알리기
        StartCoroutine(CheckNoise(5f));

        int gageCnt = _gaugeBlocks.Length;

        for (int i = 0; i < gageCnt; i++)
        {
            await Task.Delay(5500 / (gageCnt + 1));
            _gaugeBlocks[i].SetActive(true);
        }
        await Task.Delay(5500 / (gageCnt + 1) + 5500 % (gageCnt + 1));

        _correctionPanel.SetActive(false);
        endEvent?.Invoke();
    }
    
    IEnumerator CheckNoise(float duration)
    {
        float curTime = 0f;
        float sumVolume = 0f;
        int sumCount = 0;

        MicrophoneInputAnalyzer microphoneAnalyzer = GameManager.Instance.microphoneInputAnalyzer;
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
        float newNoiseVolume = (sumVolume / sumCount) + 0.04f + _sensitivity;
        microphoneAnalyzer.SetNoiseVolume(newNoiseVolume);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void SetSensitivity(float amount)
    {
        _sensitivity = amount / 50;
    }
}
