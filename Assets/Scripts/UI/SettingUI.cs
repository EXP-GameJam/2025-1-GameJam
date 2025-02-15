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

        string name = SceneManager.GetActiveScene().name;
        if (name == "TitleScene")
        {
            SoundManager.Instance.PlaySquareButtonSound();
        }
        else if (name == "GameScene")
        {
            SoundManager.Instance.PlaySmallButtonSound();
        }
    }

    public void CloseSettingPanel()
    {
        SoundManager.Instance.PlaySmallButtonSound();
        CloseSettingPannelNoSound();
    }

    private void CloseSettingPannelNoSound()
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
        SoundManager.Instance.PlaySmallButtonSound();

        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            _correctionUI.StartCorrection(() => { CloseSettingPannelNoSound(); _correctionUI.StartGame(); });
        }
        else if (SceneManager.GetActiveScene().name == "GameScene")
        {
            _correctionUI.StartCorrection(CloseSettingPannelNoSound);
        }
        else
        {
            Debug.Log("오류! 이상한 씬에 있음");
        }
    }

    public void GoToTitle()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("TitleScene");
        SoundManager.Instance.PlaySquareButtonSound();
    }
}
