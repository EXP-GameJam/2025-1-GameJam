using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private CorrectionUI _correctionUI;

    [SerializeField] private GameObject _settingPanel;
    [SerializeField] private GameObject[] _hideElements;


    public void ShowSettingPanel()
    {
        _settingPanel.SetActive(true);

        foreach (GameObject go in _hideElements)
        {
            go.SetActive(false);
        }

        Time.timeScale = 0;
    }

    public void CloseSettingPanel()
    {
        _settingPanel.SetActive(false);

        foreach (GameObject go in _hideElements)
        {
            go.SetActive(true);
        }

        Time.timeScale = 1;
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
}
