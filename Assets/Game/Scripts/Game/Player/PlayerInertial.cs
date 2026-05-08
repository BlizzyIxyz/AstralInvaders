using UnityEngine;

public class PlayerInertial : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Camera _camera;
    [SerializeField] private PlayerLaserBeam _laserBeam;
    [SerializeField] private PlayerUpgrades _playerUpgrades;
    [SerializeField] private Health _health;

    [SerializeField] private float _maxSpeed = 6f;
    [SerializeField] private float _moveForce = 300f;
    [SerializeField] private float _friction = 0.3f;
    [SerializeField] private float _recoilForce = 0.3f;

    private bool _enabled;

    private void Awake()
    {
        _input.OnRMB += ShotRMB;
        _laserBeam.OnShotEnd += Enable;
        _laserBeam.OnShotStart += ApplyRecoilForce;
    }

    private void Update()
    {
        if (_enabled)
        {
            Move();
            Rotate();
        }
        ApplyFriction();
        ClampSpeed();
    }

    public void Disable()
    {
        _enabled = false;
        _input.Disable();
    }

    public void Enable()
    {
        _enabled = true;
        _input.Enable();
    }

    private void ShotRMB()
    {
        if (!_playerUpgrades.HasLaserBeamLUpgrade)
            return;

        Disable();
        _laserBeam.Shoot();
    }

    private void ApplyRecoilForce()
    {
        //Well this will not do much but ok :D
        _rb.AddForce(_recoilForce * transform.TransformDirection(Vector2.left), ForceMode2D.Impulse);
    }

    private void Move()
    {
        _rb.AddForce(_input.MoveInput * _moveForce * Time.deltaTime * 10f);
    }

    private void Rotate()
    {
        Vector3 mousePosition = _camera.ScreenToWorldPoint(_input.PointerPosition);

        Vector2 direction = (mousePosition - transform.position).normalized;

        transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    }

    private void ApplyFriction()
    {
        if (_input.MoveInput.sqrMagnitude < 0.01f)
        {
            _rb.linearVelocity = Vector2.Lerp(_rb.linearVelocity, Vector2.zero, _friction * Time.deltaTime * 10f);
        }
    }

    private void ClampSpeed()
    {
        if (_rb.linearVelocity.sqrMagnitude >= _maxSpeed * _maxSpeed)
        {
            _rb.linearVelocity = _rb.linearVelocity.normalized * _maxSpeed;
        }
    }
}
