using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private Image _accelGauge;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private TextMeshProUGUI _finalScoreText;

    private Rocket _rocket;

    private void Start()
    {
        _rocket = FindObjectsOfType<Rocket>()[0];
        GameManager.Instance._ingameManager.OnScoreChanged += UpdateScore;
        GameManager.Instance._ingameManager.OnGameEnd += ShowGameOverPanel;
    }

    private void Update()
    {
        SetAccelGaugeFillAmount(_rocket.GetDeltaRMS());
    }

    private void OnDisable()
    {
        GameManager.Instance._ingameManager.OnGameEnd -= ShowGameOverPanel;
    }

    private void UpdateScore(int score)
    {
        _currentScoreText.text = "score : " + score.ToString();
    }

    public void SetAccelGaugeFillAmount(float amount)
    {
        _accelGauge.fillAmount = Mathf.Clamp(amount, 0, 14) / 14;
        Debug.Log($"{Mathf.Clamp(amount, 0, 14) / 14}");
    }

    public void ShowGameOverPanel(int score)
    {
        _gameOverPanel.SetActive(true);
        _finalScoreText.text = score.ToString();
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
}
