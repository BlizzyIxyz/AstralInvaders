using UnityEngine;

public abstract class BossAttack : MonoBehaviour
{
    [field: SerializeField] public float DelayAfterWarning { get; private set; }
    [field: SerializeField] public float DelayAfterAttack { get; private set; }

    public abstract void Prepare(Transform boss, Transform player);
    public abstract void Execute(Transform boss, Transform player);
}