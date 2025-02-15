using UnityEngine;
using UnityEngine.UI;

public class GuideUI : MonoBehaviour
{
    [SerializeField] private GameObject _guidePanel;
    [SerializeField] private GameObject[] _pages;

    [SerializeField] private Button _prevButton;
    [SerializeField] private Button _nextButton;

    [SerializeField] private Sprite _prevSpriteOn;
    [SerializeField] private Sprite _prevSpriteOff;
    [SerializeField] private Sprite _nextSpriteOn;
    [SerializeField] private Sprite _nextSpriteOff;

    private int _pageIndex = 0;
    private Image _prevImage;
    private Image _nextImage;

    private void Awake()
    {
        _prevImage = _prevButton.GetComponent<Image>();
        _nextImage = _nextButton.GetComponent<Image>();

        _prevButton.onClick.AddListener(ShowPrev);
        _nextButton.onClick.AddListener(ShowNext);
    }

    public void OpenGuidePanel()
    {
        _pageIndex = 0;

        foreach (GameObject page in _pages)
        {
            page.SetActive(false);
        }

        _pages[_pageIndex].SetActive(true);
        _prevButton.interactable = false;
        _prevImage.sprite = _prevSpriteOff;

        _guidePanel.SetActive(true);

        SoundManager.Instance.PlaySquareButtonSound();
    }

    public void ShowNext()
    {
        _pages[_pageIndex++].SetActive(false);
        _pages[_pageIndex].SetActive(true);

        if (_pageIndex == _pages.Length - 1)
        {
            _nextButton.interactable = false;
            _nextImage.sprite = _nextSpriteOff;
        }

        _prevButton.interactable = true;
        _prevImage.sprite = _prevSpriteOn;

        SoundManager.Instance.PlaySmallButtonSound();
    }

    public void ShowPrev()
    {
        _pages[_pageIndex--].SetActive(false);
        _pages[_pageIndex].SetActive(true);

        if (_pageIndex == 0)
        {
            _prevButton.interactable = false;
            _prevImage.sprite = _prevSpriteOff;
        }

        _nextButton.interactable = true;
        _nextImage.sprite = _nextSpriteOn;

        SoundManager.Instance.PlaySmallButtonSound();
    }
}
