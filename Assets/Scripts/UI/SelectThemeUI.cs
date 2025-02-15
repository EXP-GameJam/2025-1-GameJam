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
    [SerializeField] float _selectedSizeRate = 1.6f;
    [SerializeField] bool _isFlip;

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

        _prevButton.onClick.AddListener(() => SetCurrentSelectScale(1));
        _prevButton.onClick.AddListener(SelectPrev);
        _nextButton.onClick.AddListener(() => SetCurrentSelectScale(1));
        _nextButton.onClick.AddListener(SelectNext);
    }

    private void InitUI()
    {
        _selectElements = new List<Transform>();
        foreach (Transform t in _elementContent)
        {
            ChangeScale(t, 1);
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
            _selectElements.Add(Instantiate(_selectElements[0], _elementContent).transform);
        }

        _selectElements[_currentIdx].localPosition = Vector3.zero;
        _selectElements[ClampIndex(_currentIdx + 1)].localPosition = Vector3.right * _elementDistance;
        _selectElements[ClampIndex(_currentIdx - 1)].localPosition = Vector3.left * _elementDistance;
        for (int i = 0; i < _selectElements.Count - 3; i++)
        {
            // 나머지 요소들은 보이지않게 위로 치우기
            _selectElements[ClampIndex(_currentIdx + 2 + i)].localPosition = Vector3.up * _elementDistance;
        }

        SetCurrentSelectScale(_selectedSizeRate);
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

        SetCurrentSelectScale(_selectedSizeRate);
    }

    private void SetCurrentSelectScale(float amount)
    {
        Debug.Log("change");
        ChangeScale(_selectElements[_currentIdx], amount);
    }

    private void ChangeScale(Transform transform, float amount)
    {
        transform.localScale = new Vector3(((_isFlip) ? -1 : 1) * amount, amount, 1);
    }

    private int ClampIndex(int index)
    {
        return (index + _selectElements.Count) % _selectElements.Count;
    }
}
