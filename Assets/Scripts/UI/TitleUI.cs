using UnityEngine;
using UnityEngine.EventSystems;

public class TitleUI : MonoBehaviour
{
    [SerializeField] private CorrectionUI _correctionUI;

    [SerializeField] private GameObject _selectThemePanel;
    [SerializeField] private GameObject _gameWayPanel;

    public void StartGame()
    {
        _correctionUI.StartCorrection(_correctionUI.StartGame);
        /*
        if (GameManager.Instance.microphoneInputAnalyzer.hasBaseVolume)
        {
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            _correctionUI.StartCorrection(_correctionUI.StartGame);
        }*/
    }

    public void ShowSelectThemePanel() => _selectThemePanel.SetActive(true);

    public void ShowGameWayPanel() => _gameWayPanel.SetActive(true);

    public void ClosePanel()
    {
        EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);
    }
}
