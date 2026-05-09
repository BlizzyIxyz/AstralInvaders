using UnityEngine;

public class SingleAudioSource : MonoBehaviour
{
    [field: SerializeField] public AudioSource AudioSource { get; private set; }

    public static SingleAudioSource Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}