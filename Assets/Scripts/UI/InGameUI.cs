using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
        GameManager.Instance._ingameManager.OnGameEnd += ShowGameOverPanel;
    }

    private void OnDisable()
    {
        GameManager.Instance._ingameManager.OnGameEnd -= ShowGameOverPanel;
    }

    /// <param name="amount"> 0 ~ 1 </param>
    public void SetAccelGaugeFillAmount(float amount)
    {
        _accelGauge.fillAmount = amount;
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
