using System.Collections;
using UnityEngine;

public class DeathParticle : MonoBehaviour
{
    [SerializeField] private AnimationClip _animClip;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource.PlayOneShot(_audioClip);
        StartCoroutine(DestroyAfterPlayCoroutine());
    }

    private IEnumerator DestroyAfterPlayCoroutine()
    {
        yield return new WaitForSeconds(_animClip.length);

        Destroy(gameObject);
    }
}
