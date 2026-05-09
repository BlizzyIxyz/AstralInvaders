using System.Linq;
using UnityEngine;

public class PassiveShooter : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private EnemyAggregator _enemyAggregator;
    [SerializeField] private float _attackCooldown = 0.5f;

    private float _currentCooldown;

    private void Update()
    {
        if (_currentCooldown > 0)
        {
            _currentCooldown -= Time.deltaTime;
            return;
        }

        Enemy target = FindNearestEnemyOnScreen();
        if (target != null)
        {
            ShootAt(target);
            _currentCooldown = _attackCooldown;
        }
    }

    private void ShootAt(Enemy target)
    {
        Vector2 direction = (target.Position - transform.position).normalized;

        GameObject projectileObject = Instantiate(
            _projectilePrefab,
            transform.position,
            Quaternion.identity
        );

        if (direction.x < 0)
        {
            projectileObject.transform.localScale = new Vector3(
                -Mathf.Abs(projectileObject.transform.localScale.x),
                projectileObject.transform.localScale.y,
                projectileObject.transform.localScale.z
            );
        }
        else
        {
            projectileObject.transform.localScale = new Vector3(
                Mathf.Abs(projectileObject.transform.localScale.x),
                projectileObject.transform.localScale.y,
                projectileObject.transform.localScale.z
            );
        }

        PlayerProjectile projectile = projectileObject.GetComponent<PlayerProjectile>();
        if (projectile != null)
        {
            projectile.Launch(direction);
        }
    }

    private Enemy FindNearestEnemyOnScreen()
    {
        return _enemyAggregator
            .GetEnemies()
            .Where(e => e.IsWithingScreen && !e.IsDead)
            .OrderBy(e => Vector2.Distance(transform.position, e.Position))
            .FirstOrDefault();
    }
}