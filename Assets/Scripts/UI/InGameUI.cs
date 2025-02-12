using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private Image _accelGauge;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private void OnEnable()
    {
        GameManager.Instance._ingameManager.OnRMSChanged += SetAccelGaugeFillAmount;
        GameManager.Instance._ingameManager.OnGameEnd += ShowGameOverPanel;
    }

    private void OnDisable()
    {
        GameManager.Instance._ingameManager.OnRMSChanged -= SetAccelGaugeFillAmount;
        GameManager.Instance._ingameManager.OnGameEnd -= ShowGameOverPanel;
    }

    public void SetAccelGaugeFillAmount(float amount)
    {
        _accelGauge.fillAmount = Mathf.Clamp(amount, 0, 0.08f) / 0.08f;
    }

    public void ShowGameOverPanel(int score)
    {
        _gameOverPanel.SetActive(true);
        _scoreText.text = "Your Score : " + score.ToString();
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
