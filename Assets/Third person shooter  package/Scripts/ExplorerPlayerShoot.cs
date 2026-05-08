using UnityEngine;
using UnityEngine.InputSystem;

public class ExplorerPlayerShoot : MonoBehaviour
{
    [Header("Shooting")]
    public float damage = 25f;
    public float range = 100f;
    public float fireRate = 0.5f;
    public LayerMask enemyLayers;

    [Header("Effects")]
    public ParticleSystem muzzleFlash;
    public GameObject hitEffect;
    public AudioClip shootSound;

    private float _nextFireTime;
    private AudioSource _audioSource;
    private Camera _mainCamera;
    private Animator _animator;
    private int _animFire;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            _audioSource = gameObject.AddComponent<AudioSource>();

        _mainCamera = Camera.main;
        _animator = GetComponentInChildren<Animator>();
        _animFire = Animator.StringToHash("Fire");
    }

    void Update()
    {
        // Use Update instead of Input callback
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryShoot();
        }
    }

    void TryShoot()
    {
        if (Time.time < _nextFireTime) return;
        _nextFireTime = Time.time + fireRate;

        // Muzzle flash
        if (muzzleFlash != null)
            muzzleFlash.Play();

        // Shoot sound
        if (shootSound != null && _audioSource != null)
            _audioSource.PlayOneShot(shootSound);

        // Fire animation
        if (_animator != null)
            _animator.SetTrigger(_animFire);

        // Raycast from camera center
        if (_mainCamera == null) return;

        Ray ray = _mainCamera.ScreenPointToRay(
            new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

        Debug.DrawRay(ray.origin, ray.direction * range, Color.red, 1f);

        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            Debug.Log("Hit: " + hit.collider.gameObject.name);

            // Hit effect
            if (hitEffect != null)
                Instantiate(hitEffect, hit.point, 
                    Quaternion.LookRotation(hit.normal));

            // Damage zombie
            ZombieAI zombie = hit.collider.GetComponentInParent<ZombieAI>();
            if (zombie != null)
            {
                Debug.Log("Zombie hit!");
                zombie.Die();
            }
        }
    }
}
