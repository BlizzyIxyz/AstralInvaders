using UnityEngine;
using DG.Tweening;

public class PlayerCentralizer : MonoBehaviour
{
    [SerializeField] private float _centralizeTime;

    private Tweener _tweener;

    public void Centralize()
    {
        _tweener?.Kill();
        Vector3 target = new Vector3(0f, 0f, transform.position.z);
        _tweener = transform.DOMove(target, _centralizeTime).SetEase(Ease.OutCubic);
    }
}