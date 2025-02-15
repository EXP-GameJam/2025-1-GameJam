using UnityEngine;
using UnityEngine.EventSystems;

public class SoundSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEndDragHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        SoundManager.Instance.PlayVolumeSound(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SoundManager.Instance.PlayVolumeSound();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SoundManager.Instance.PlayVolumeSound();
    }

}
