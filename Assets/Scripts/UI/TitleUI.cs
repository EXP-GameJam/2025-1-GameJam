using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    [SerializeField] private CorrectionUI _correctionUI;

    [SerializeField] private GameObject _selectThemePanel;
    [SerializeField] private GameObject _gameWayPanel;

    public void StartGame()
    {
        if (GameManager.Instance.microphoneInputAnalyzer.hasBaseVolume)
        {
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            _correctionUI.StartCorrection(_correctionUI.StartGame);
        }
    }

    public void ShowSelectThemePanel()
    {
        _selectThemePanel.SetActive(true);
    }

    public void ClosePanel()
    {
        EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);
    }
}
