using System.Collections;
using UnityEngine;

public class AirAttack : BossAttack
{
    [SerializeField] private GameObject[] _laserWarnings;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private float _attackInterval = 1f;
    [SerializeField] private GameObject _impactEffect;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _attackClip;
    [SerializeField] private float _attackRadius;

    public override void Execute(Transform boss, Transform player)
    {

    }

    public override void Prepare(Transform boss, Transform player)
    {
        StartCoroutine(StartAttackSequence(player));
    }

    private IEnumerator StartAttackSequence(Transform player)
    {
        for (int i = 0; i < _laserWarnings.Length; i++)
        {
            _laserWarnings[i].transform.position = player.position;
            _laserWarnings[i].SetActive(true);

            StartCoroutine(DisableAfter(_laserWarnings[i], DelayAfterWarning));
            StartCoroutine(ExecuteAttack(_laserWarnings[i].transform.position, DelayAfterWarning));

            if (i < _laserWarnings.Length - 1)
                yield return new WaitForSeconds(_attackInterval);
        }
    }

    private IEnumerator DisableAfter(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }

    private IEnumerator ExecuteAttack(Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);

        Collider2D[] hits = Physics2D.OverlapCircleAll(position, _attackRadius, _targetLayer);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Health>(out Health health))
            {
                health.ReduceHP(1);
            }
        }

        if (_impactEffect != null)
        {
            Instantiate(_impactEffect, position, Quaternion.identity);
        }
        PlayAudio();
    }

    private void PlayAudio()
    {
        _source.Stop();
        _source.clip = _attackClip;
        _source.Play();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = Color.greenYellow;
        Gizmos.DrawWireSphere(transform.position, _attackRadius);
    }
#endif
}