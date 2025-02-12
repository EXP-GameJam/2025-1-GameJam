using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CorrectionUI : MonoBehaviour
{
    [SerializeField] private GameObject _correctionPanel;

    public delegate void CorrectionEndEvent();

    /// <summary> ���� ���� </summary>
    public void StartCorrection(CorrectionEndEvent endEvent)
    {
        _correctionPanel.SetActive(true);

        // ����

        // �����Ϸ��Ѱ� GameManger���� �˸���
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
