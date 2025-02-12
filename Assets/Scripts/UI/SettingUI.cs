using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private CorrectionUI _correctionUI;

    [SerializeField] private GameObject _settingPanel;


    public void ShowSettingPanel()
    {
        _settingPanel.SetActive(true);
    }

    public void StartCorrection()
    {
        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            _correctionUI.StartCorrection(_correctionUI.StartGame);
        }
        else if (SceneManager.GetActiveScene().name == "GameScene")
        {
            _correctionUI.StartCorrection(_correctionUI.CloseCorrectionPanel);
            CloseSettingPanel();
        }
        else
        {
            Debug.Log("오류! 이상한 씬에 있음");
        }
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void CloseSettingPanel()
    {
        _settingPanel.SetActive(false);
    }
}
