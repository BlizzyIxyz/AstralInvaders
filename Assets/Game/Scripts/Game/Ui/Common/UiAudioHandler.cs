using UnityEngine;

public class UiAudioHandler : MonoBehaviour
{
    [SerializeField] protected AudioSource _audioSource;
    [SerializeField] protected AudioClip _onPointerEnterClip;
    [SerializeField] protected AudioClip _onPointerExitClip;
    [SerializeField] protected AudioClip _onPointerDownClip;
    [SerializeField] protected AudioClip _onPointerUpClip;

    public void PlayOnPointerEnterClip()
    {
        if (_onPointerEnterClip == null)
            return;

        _audioSource.Stop();
        _audioSource.PlayOneShot(_onPointerEnterClip);
    }
    public void PlayOnPointerExitClip()
    {
        if (_onPointerExitClip == null)
            return;

        _audioSource.Stop();
        _audioSource.PlayOneShot(_onPointerExitClip);
    }
    public void PlayOnPointerDownClip()
    {
        if (_onPointerDownClip == null)
            return;

        _audioSource.Stop();
        _audioSource.PlayOneShot(_onPointerDownClip);
    }
    public void PlayOnPointerUpClip()
    {
        if (_onPointerUpClip == null)
            return;

        _audioSource.Stop();
        _audioSource.PlayOneShot(_onPointerUpClip);
    }
}