using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonImageHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Sprite _pointerUpSprite;
    [SerializeField] private Sprite _pointerDownSprite;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.sprite = _pointerUpSprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        image.sprite = _pointerDownSprite;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        image.sprite= _pointerUpSprite;
    }
}
