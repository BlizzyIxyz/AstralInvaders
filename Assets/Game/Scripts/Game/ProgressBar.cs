using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _barRenderer;
    [SerializeField] private Sprite[] _stageSprites;
    [SerializeField] private Transform _target;

    private Quaternion _initRotation;
    private Vector3 _initLocalPosition;

    private void Awake()
    {
        _initRotation = transform.rotation;
        _initLocalPosition = transform.position - _target.position;
    }

    private void LateUpdate()
    {
        transform.rotation = _initRotation;
        transform.position = _initLocalPosition + _target.position;
    }

    public void SetProgress(float current, float max)
    {
        if (_barRenderer == null || _stageSprites == null || _stageSprites.Length == 0)
            return;

        if (max <= 0) return;

        float progress = current / max;
        int spriteIndex = Mathf.Clamp(Mathf.FloorToInt(progress * 10), 0, 9);

        if (spriteIndex < _stageSprites.Length)
            _barRenderer.sprite = _stageSprites[spriteIndex];
    }
}