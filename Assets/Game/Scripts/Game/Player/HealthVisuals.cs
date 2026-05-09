using UnityEngine;

public class HealthVisuals : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private GameObject[] _sprites;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    private float _lastHealth;

    private void Awake()
    {
        _health.OnHealthChange += HandleHealthChange;
    }

    private void HandleHealthChange(float health)
    {
        _lastHealth = health;

        if (_lastHealth > health)
        {
            _audioSource.PlayOneShot(_audioClip);
        }

        switch (health)
        {
            case 0:
                _sprites[0].gameObject.SetActive(false); 
                _sprites[1].gameObject.SetActive(false); 
                _sprites[2].gameObject.SetActive(false); 
                break;
            case 1:
                _sprites[0].gameObject.SetActive(true); 
                _sprites[1].gameObject.SetActive(false); 
                _sprites[2].gameObject.SetActive(false); 
                break;
            case 2:
                _sprites[0].gameObject.SetActive(true); 
                _sprites[1].gameObject.SetActive(true); 
                _sprites[2].gameObject.SetActive(false); 
                break;
            case 3:
                _sprites[0].gameObject.SetActive(true); 
                _sprites[1].gameObject.SetActive(true); 
                _sprites[2].gameObject.SetActive(true); 
                break;
        }
    }
}
