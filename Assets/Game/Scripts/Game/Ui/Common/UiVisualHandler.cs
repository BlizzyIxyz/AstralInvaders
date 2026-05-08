using UnityEngine;
using UnityEngine.EventSystems;

public class UiVisualHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private UiAudioHandler _uiAudioHandler;
    [SerializeField] private UiAnimationHandler _uiAnimationHandler;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _uiAudioHandler?.PlayOnPointerEnterClip();
        _uiAnimationHandler?.PlayOnPointerEnterClip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _uiAudioHandler?.PlayOnPointerExitClip();
        _uiAnimationHandler?.PlayOnPointerExitClip();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _uiAudioHandler?.PlayOnPointerDownClip();
        _uiAnimationHandler?.PlayOnPointerDownClip();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _uiAudioHandler?.PlayOnPointerUpClip();
        _uiAnimationHandler?.PlayOnPointerUpClip();
    }
}
