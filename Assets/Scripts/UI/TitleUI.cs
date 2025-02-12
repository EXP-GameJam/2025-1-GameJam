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
        if (GameManager.Instance.microphoneInputAnalyzer.hasBaseVolume)
        {
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            _correctionUI.StartCorrection(_correctionUI.StartGame);
        }
    }

    public void ShowSelectThemePanel() => _selectThemePanel.SetActive(true);

    public void ShowGameWayPanel() => _gameWayPanel.SetActive(true);

    public void ClosePanel()
    {
        EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);
    }

    public void SelectTheme()
    {
        _characterTheme.GetIndex();
        _coinTheme.GetIndex(); // TODO => 캐릭터 이미지랑 연결
        ClosePanel();
    }
}
