using UnityEngine;
using DG.Tweening;

public class GameSlower : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _transitionDuration = 1f;

    private Tween _timeTween;

    public void SlowDown(float targetScale)
    {
        _timeTween?.Kill();

        _timeTween = DOTween.To(
            () => Time.timeScale,
            x => Time.timeScale = x,
            targetScale,
            _transitionDuration
        ).SetEase(Ease.OutQuart)
         .SetUpdate(true);
    }

    public void SpeedUp()
    {
        _timeTween?.Kill();

        _timeTween = DOTween.To(
            () => Time.timeScale,
            x => Time.timeScale = x,
            1f,
            _transitionDuration
        ).SetEase(Ease.InQuart)
         .SetUpdate(true);
    }

    private void OnDestroy()
    {
        _timeTween?.Kill();
        Time.timeScale = 1f;
    }
}