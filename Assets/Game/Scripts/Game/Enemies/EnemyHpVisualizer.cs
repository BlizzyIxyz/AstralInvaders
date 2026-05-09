using UnityEngine;

public class EnemyHpVisualizer : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private ProgressBar _progressBar;

    private void Awake()
    {
        UpdateProgressBar(0);

        _health.OnHealthChange += UpdateProgressBar; 
    }

    private void UpdateProgressBar(float hp)
    {
        _progressBar.SetProgress(_health.HP, _health.InitHp);
    }
}
