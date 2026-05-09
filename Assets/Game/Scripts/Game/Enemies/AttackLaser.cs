using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class AttackLaser : BossAttack
{
    [SerializeField] private GameObject[] _laserWarnings;
    [SerializeField] private PlayableDirector[] _lasers;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _attackClip;

    public override void Execute(Transform boss, Transform player)
    {
        foreach (var director in _lasers)
        {
            director.Play();
        }
        PlayAudio();
    }

    public override void Prepare(Transform boss, Transform player)
    {
        foreach (var laserWarning in _laserWarnings)
        {
            laserWarning.SetActive(true);
            StartCoroutine(DisableAfter(laserWarning, DelayAfterWarning));
        }
    }

    private IEnumerator DisableAfter(GameObject @object, float time)
    {
        yield return new WaitForSeconds(time);
        @object.SetActive(false);
    }

    private void PlayAudio()
    {
        _source.Stop();
        _source.clip = _attackClip;
        _source.Play();
    }
}
