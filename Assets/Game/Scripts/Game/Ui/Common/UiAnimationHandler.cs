using UnityEngine;

public abstract class UiAnimationHandler : MonoBehaviour
{
    public abstract void PlayOnPointerEnterClip();
    public abstract void PlayOnPointerExitClip();
    public abstract void PlayOnPointerDownClip();
    public abstract void PlayOnPointerUpClip();
}