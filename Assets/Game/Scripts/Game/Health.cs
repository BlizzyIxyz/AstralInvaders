using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action OnHealthChange;
    public event Action OnDeath;

    [SerializeField] private float _initialHP;

    [Header("Invulnerability Settings")]
    [SerializeField] private float _invulnerabilityDuration = 1f;
    [SerializeField] private bool _activateInvulnerability;

    [field: SerializeField] public float HP { get; private set; }
    public bool IsDead { get; private set; } = false;
    public bool IsInvulnerable { get; private set; } = false;

    private void Awake()
    {
        HP = _initialHP;
    }

    public void ReduceHP(float hp)
    {
        if (hp <= 0 || IsDead || IsInvulnerable)
            return;

        HP -= hp;

        HP = Mathf.Clamp(HP, 0, _initialHP);

        OnHealthChange?.Invoke();

        if (_activateInvulnerability)
        {
            SetInvulnerability(_invulnerabilityDuration);
        }

        if (HP == 0)
        {
            IsDead = true;
            OnDeath?.Invoke();
        }
    }

    public void IncreaseHP(float hp)
    {
        if (hp <= 0 || IsDead)
            return;

        HP += hp;

        HP = Mathf.Clamp(HP, 0, _initialHP);

        OnHealthChange?.Invoke();
    }

    public void Kill()
    {
        if (IsDead) return;

        IsDead = true;
        HP = 0;
        OnDeath?.Invoke();
    }

    public void SetInvulnerability(float duration)
    {
        if (IsDead) return;

        StopAllCoroutines();
        StartCoroutine(InvulnerabilityRoutine(duration));
    }

    private IEnumerator InvulnerabilityRoutine(float duration)
    {
        IsInvulnerable = true;
        yield return new WaitForSeconds(duration);
        IsInvulnerable = false;
    }
}