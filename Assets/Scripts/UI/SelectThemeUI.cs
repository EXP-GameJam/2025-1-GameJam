using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectThemeUI : MonoBehaviour
{
    [SerializeField] private RectTransform _elementContent;
    [SerializeField] private Button _prevButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] float _elementDistance;

    [SerializeField] private List<Transform> _selectElements;

    [SerializeField] int _currentIdx = 0;

    private Coroutine _moveCoroutine;
    private int _contentPosIdx = 0;

    bool _isDuplicated = false;
    bool _isElementOne = false;

    // Start is called before the first frame update
    void Start()
    {
        InitUI();

        _prevButton.onClick.AddListener(() => SetCurrentSelectAlpha(0.5f));
        _prevButton.onClick.AddListener(SelectPrev);
        _nextButton.onClick.AddListener(() => SetCurrentSelectAlpha(0.5f));
        _nextButton.onClick.AddListener(SelectNext);
    }

    private void InitUI()
    {
        _selectElements = new List<Transform>();
        foreach (Transform t in _elementContent)
        {
            ChangeAlpha(t.GetComponent<Image>(), 0.5f);
            _selectElements.Add(t);
        }

        if (_selectElements.Count < 4)
        {
            _isDuplicated = true;
            foreach (Transform t in _selectElements.ToArray())
            {
                _selectElements.Add(Instantiate(t, _elementContent).transform);
            }
        }

        if (_selectElements.Count < 3)
        {
            _isElementOne = true;
            _selectElements.Add(Instantiate(_selectElements[0], _elementContent).transform);
        }

        _selectElements[_currentIdx].localPosition = Vector3.zero;
        _selectElements[ClampIndex(_currentIdx + 1)].localPosition = Vector3.right * _elementDistance;
        _selectElements[ClampIndex(_currentIdx - 1)].localPosition = Vector3.left * _elementDistance;
        for (int i = 0; i < _selectElements.Count - 3; i++)
        {
            // 나머지 요소들은 보이지않게 위로 치우기
            _selectElements[ClampIndex(_currentIdx + 2 + i)].localPosition = Vector3.up * 200;
        }
    }

    public int GetIndex()
    {
        if (_isElementOne) return 0;
        else if (_isDuplicated) return _currentIdx / 2;
        else return _currentIdx;
    }

    private void SelectPrev()
    {
        _currentIdx = ClampIndex(--_currentIdx);
        _selectElements[ClampIndex(_currentIdx - 1)].position = _selectElements[_currentIdx].position - Vector3.right * _elementDistance;
        _contentPosIdx++;
        StartMoveElements(); 
    }

    private void SelectNext()
    {
        _currentIdx = ClampIndex(++_currentIdx);
        _selectElements[ClampIndex(_currentIdx + 1)].position = _selectElements[_currentIdx].position + Vector3.right * _elementDistance;
        _contentPosIdx--;
        StartMoveElements();
    }

    private void StartMoveElements()
    {
        if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
        StartCoroutine(MoveElements());
    }

    IEnumerator MoveElements()
    {
        float time = 0;

        Vector3 startPos = _elementContent.anchoredPosition;
        Vector3 endPos = Vector3.right * _contentPosIdx * _elementDistance;

        while ((time += Time.deltaTime) < 0.2)
        {
            _elementContent.anchoredPosition = Vector3.Lerp(startPos, endPos, time * 5);
            yield return null;
        }
        _elementContent.anchoredPosition = endPos;

        SetCurrentSelectAlpha(1);
    }

    private void SetCurrentSelectAlpha(float alpha)
    {
        ChangeAlpha(_selectElements[_currentIdx].GetComponent<Image>(), alpha);
    }

    private void ChangeAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    private int ClampIndex(int index)
    {
        return (index + _selectElements.Count) % _selectElements.Count;
    }
}
