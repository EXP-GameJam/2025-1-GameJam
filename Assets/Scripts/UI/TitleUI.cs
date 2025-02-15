using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    [SerializeField] private CorrectionUI _correctionUI;

    [SerializeField] private GameObject _selectThemePanel;
    [SerializeField] private GameObject _gameWayPanel;

    [SerializeField] private SelectThemeUI _characterTheme;
    [SerializeField] private SelectThemeUI _coinTheme;

    public void StartGame()
    {
        SoundManager.Instance.PlaySquareButtonSound();

        if (GameManager.Instance.microphoneInputAnalyzer.hasNoiseVolume)
        {
            Time.timeScale = 1;
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
        SoundManager.Instance.PlaySquareButtonSound();
    }

    public void ClosePanel()
    {
        EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);
        SoundManager.Instance.PlaySmallButtonSound();
    }

    public void SelectTheme()
    {
        _characterTheme.GetIndex();
        _coinTheme.GetIndex(); // TODO => ĳ���� �̹����� ����
        ClosePanel();
    }
}
