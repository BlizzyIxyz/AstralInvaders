using UnityEngine;

public class Damager : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerInertial>(out _))
        {
            var health = collision.gameObject.GetComponent<Health>();
            if (!health.IsDead)
            {
                health.ReduceHP(1);
            }
        }
    }
}