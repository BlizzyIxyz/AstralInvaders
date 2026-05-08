using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Wave", fileName = "WaveConfig")]
public class Wave : ScriptableObject
{
    [field: SerializeField] public WaveStep[] Steps { get; private set; }
    [field: SerializeField] public float DelayAfter { get; private set; }
}