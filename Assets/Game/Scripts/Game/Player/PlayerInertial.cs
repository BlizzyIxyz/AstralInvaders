using UnityEngine;
using System.Collections;
using UnityEngine.Playables;

public class PlayerInertial : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Camera _camera;
    [SerializeField] private PlayerLaserBeam _laserBeam;
    [SerializeField] private PlayerUpgrades _playerUpgrades;
    [SerializeField] private Health _health;
    [SerializeField] private TutorialController _tutorialController;

    [Header("Slow Motion")]
    [SerializeField] private GameSlower _gameSlower;
    [SerializeField] private float _slowMotionScale = 0.3f;
    [SerializeField] private float _slowMotionDuration = 0.5f;

    [Header("Movement Settings")]
    [SerializeField] private float _maxSpeed = 6f;
    [SerializeField] private float _moveForce = 300f;
    [SerializeField] private float _friction = 0.3f;
    [SerializeField] private float _recoilForce = 0.3f;

    [Header("Attack Settings")]
    [SerializeField] private PlayerAttack _attack;

    [Header("RMB Settings")]
    [SerializeField] private float _rmbCooldown = 1.5f;
    [SerializeField] private ProgressBar _rmbProgressBar;
    [SerializeField] private PlayableDirector _deathCutscene;
    [SerializeField] private PlayableDirector _winCutscene;
    private float _rmbCooldownTimer;

    private bool _enabled;
    private float _attackTimer;

    public void SetAttackStrategy(PlayerAttack playerAttack)
    {
        _attack = playerAttack;
    }

    private void Awake()
    {
        _input.OnRMB += ShotRMB;

        _laserBeam.OnShotEnd += Enable;
        _laserBeam.OnShotStart += ApplyRecoilForce;

        _tutorialController.TutorialComplete += _health.DisableInvulnerability;

        _health.OnHealthChange += HandleHealthChange;
    }

    private void Update()
    {
        if (_rmbCooldownTimer > 0)
        {
            _rmbCooldownTimer -= Time.deltaTime;
            _rmbProgressBar.SetProgress(_rmbCooldownTimer, _rmbCooldown);
        }

        if (_attackTimer > 0)
        {
            _attackTimer -= Time.deltaTime;
        }

        if (_enabled)
        {
            Move();
            Rotate();

            if (_input.IsLMBPressed)
            {
                HandleLMB();
            }
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

    private void HandleLMB()
    {
        if (_attack == null) return;

        if (_attackTimer > 0) return;

        _attack.Execute(transform);
        _attackTimer = _attack.Cooldown;
    }

    private void ShotRMB()
    {
        if (_rmbCooldownTimer > 0)
            return;

        if (!_playerUpgrades.HasLaserBeamLUpgrade)
            return;

        Disable();
        _laserBeam.Shoot();

        _rmbCooldownTimer = _rmbCooldown;
    }

    private void ApplyRecoilForce()
    {
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

    private void HandleHealthChange(float health)
    {
        if (health != 0)
        {
            _gameSlower.SlowDown(_slowMotionScale);
            StartCoroutine(RestoreTimeRoutine());
        }
        else
            _deathCutscene.Play();
    }

    private IEnumerator RestoreTimeRoutine()
    {
        yield return new WaitForSecondsRealtime(_slowMotionDuration);
        _gameSlower.SpeedUp();
    }
}