using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraShaker : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private float _strength = 3f;
    [SerializeField] private int _vibrato = 10;
    [SerializeField] private float _randomness = 90f;
    [SerializeField] private bool _fadeOut = true;

    private Camera _camera;
    private Tween _currentShake;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    public void Shake()
    {
        _currentShake?.Kill(true);

        _currentShake = _camera.transform.DOShakePosition(
            _duration,
            _strength,
            _vibrato,
            _randomness,
            _fadeOut
        );
    }

    public void KillShaker()
    {
        _currentShake?.Kill(true);
    }
}