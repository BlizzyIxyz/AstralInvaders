using UnityEngine;

[CreateAssetMenu(menuName = "Player/Attack/Projectile", fileName = "NewPlayerAttackProjectile")]
public class PlayerAttackProjectile : PlayerAttack
{
    [SerializeField] private PlayerProjectile _projectilePrefab;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private Vector2 _launchDirection = Vector2.right;

    public override void Execute(Transform player)
    {
        if (_projectilePrefab == null) return;

        PlayerProjectile newProjectile = Instantiate(_projectilePrefab, player.position, player.rotation);

        Vector2 worldDirection = player.TransformDirection(_launchDirection);
        newProjectile.Launch(worldDirection);

        SingleAudioSource.Instance.AudioSource.PlayOneShot(_clip);
    }
}