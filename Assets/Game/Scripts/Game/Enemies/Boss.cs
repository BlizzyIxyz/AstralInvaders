using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Boss : MonoBehaviour
{
    [System.Serializable]
    public struct Entry
    {
        public GameObject GameObject;
        public Health Health;
    }

    [Header("Movement")]
    [SerializeField] private Transform _eyesParent;
    [SerializeField] private Transform[] _eyes;
    [SerializeField] private float _eyesRotationSpeed;
    [SerializeField] private float _rotationSpeed;

    [Header("Parts Settings")]
    [SerializeField] private Entry[] _entries;
    [SerializeField] private AudioClip _destroySound;
    [SerializeField] private AudioSource _audioSource;

    [Header("Death Settings")]
    [SerializeField] private GameObject _explosionPrefab;

    [Header("Attack Settings")]
    [SerializeField] private BossAttack[] _attacks;
    [SerializeField] private int _loopsCount = 3;

    [Header("Entry Settings")]
    [SerializeField] private float _entrySpeed = 2f;
    [SerializeField] private GameObject _entryEffectPrefab;

    private bool _isDead = false;
    private bool _isActivated = false;
    private Coroutine _attackCoroutine;

    private Transform _playerTransform;
    private GameObject _upgradeWindow;

    private void Awake()
    {
        foreach (var entry in _entries)
        {
            if (entry.Health != null)
            {
                entry.Health.OnDeath += () => HandlePartDeath(entry);
            }
        }
    }

    public void Activate(Transform playerTransform, GameObject upgradeWindow)
    {
        if (_isActivated) return;

        _playerTransform = playerTransform;
        _upgradeWindow = upgradeWindow;

        _isActivated = true;
        StartCoroutine(EntrySequence());
    }

    private IEnumerator EntrySequence()
    {
        Vector3 targetPosition = Vector3.zero;
        float distance = Vector3.Distance(transform.position, targetPosition);
        float duration = 0f;

        GameObject entryEffect = null;
        if (_entryEffectPrefab != null)
        {
            entryEffect = Instantiate(_entryEffectPrefab, targetPosition, Quaternion.identity);
        }

        if (_entrySpeed > 0)
            duration = distance / _entrySpeed;

        yield return transform.DOMove(targetPosition, duration).SetEase(Ease.InOutCubic).WaitForCompletion();

        transform.position = targetPosition;

        if (entryEffect != null)
        {
            Destroy(entryEffect);
        }

        if (_attacks.Length > 0 && _playerTransform != null)
        {
            _attackCoroutine = StartCoroutine(AttackLoop());
        }
    }

    private IEnumerator AttackLoop()
    {
        int currentLoop = 0;

        while (!_isDead && currentLoop < _loopsCount)
        {
            foreach (var attack in _attacks)
            {
                if (_isDead) yield break;

                attack.Prepare(transform, _playerTransform);
                yield return new WaitForSeconds(attack.DelayAfterWarning);

                if (_isDead) yield break;

                attack.Execute(transform, _playerTransform);
                yield return new WaitForSeconds(attack.DelayAfterAttack);
            }

            currentLoop++;
        }
    }

    private void HandlePartDeath(Entry entry)
    {
        if (entry.GameObject != null)
        {
            entry.GameObject.SetActive(false);
        }

        if (_destroySound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(_destroySound);
        }

        CheckBossDeath();
    }

    private void CheckBossDeath()
    {
        if (_isDead) return;

        foreach (var entry in _entries)
        {
            if (entry.GameObject != null && entry.GameObject.activeInHierarchy)
            {
                return;
            }
        }

        _isDead = true;

        if (_attackCoroutine != null)
            StopCoroutine(_attackCoroutine);

        if (_explosionPrefab != null)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        }

        _upgradeWindow.SetActive(true);

        Destroy(gameObject);
    }

    private void Update()
    {
        if (!_isActivated || _isDead) return;

        Rotate();
    }

    private void Rotate()
    {
        foreach (Transform t in _eyes)
        {
            t.Rotate(new Vector3(0f, 0f, _eyesRotationSpeed * Time.deltaTime), Space.World);
        }

        _eyesParent.Rotate(new Vector3(0f, 0f, _rotationSpeed * Time.deltaTime), Space.World);
    }
}