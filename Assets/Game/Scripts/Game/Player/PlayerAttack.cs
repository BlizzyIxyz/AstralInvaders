using UnityEngine;

[CreateAssetMenu(menuName = "Player/Attack", fileName = "NewPlayerAttack")]
public abstract class PlayerAttack : ScriptableObject
{
    [SerializeField] private float _cooldown = 0.5f;

    public float Cooldown => _cooldown;

    public abstract void Execute(Transform player);
}