using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CorrectionUI : MonoBehaviour
{
    [SerializeField] private GameObject _correctionPanel;

    public delegate void CorrectionEndEvent();

    /// <summary> 보정 시작 </summary>
    public void StartCorrection(CorrectionEndEvent endEvent)
    {
        _correctionPanel.SetActive(true);

        // 보정

        // 보정완료한거 GameManger에게 알리기
        endEvent.Invoke();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void CloseCorrectionPanel()
    {
        _correctionPanel.SetActive(false);
    }
}
