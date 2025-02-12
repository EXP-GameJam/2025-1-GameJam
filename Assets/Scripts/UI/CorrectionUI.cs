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

        int gageCnt = _gaugeBlocks.Length;

        for (int i = 0; i < gageCnt; i++)
        {
            await Task.Delay(5000 / (gageCnt + 1));
            _gaugeBlocks[i].SetActive(true);
        }
        await Task.Delay(5000 / (gageCnt + 1) + 5000 % (gageCnt + 1));

        endEvent.Invoke();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void CloseCorrectionPanel()
    {
        _correctionPanel.SetActive(false);
    }
}
