using UnityEngine;
using UnityEngine.InputSystem;

public class ExplorerPlayerShoot : MonoBehaviour
{
    [Header("Shooting")]
    public float damage = 25f;
    public float range = 200f;
    public float fireRate = 0.15f;

    [Header("Hit Detection")]
    public LayerMask hitLayers;
    public float sphereCastRadius = 0.15f;

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

        // If no layers set, hit everything
        if (hitLayers == 0)
            hitLayers = ~0;
    }

    void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            TryShoot();
        }
    }

    void TryShoot()
    {
        if (Time.time < _nextFireTime) return;
        _nextFireTime = Time.time + fireRate;

        // Effects
        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (shootSound != null && _audioSource != null)
            _audioSource.PlayOneShot(shootSound);

        if (_animator != null)
            _animator.SetTrigger(_animFire);

        if (_mainCamera == null) return;

        // Get exact screen center ray
        Vector3 screenCenter = new Vector3(
            Screen.width / 2f, 
            Screen.height / 2f, 
            0f);

        Ray ray = _mainCamera.ScreenPointToRay(screenCenter);

        // Use SphereCast for more forgiving hit detection
        RaycastHit[] hits = Physics.SphereCastAll(
            ray.origin,
            sphereCastRadius,
            ray.direction,
            range,
            hitLayers);

        // Sort by distance — hit closest first
        System.Array.Sort(hits, (a, b) => 
            a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in hits)
        {
            // Skip the player itself
            if (hit.collider.transform.IsChildOf(transform) ||
                hit.collider.gameObject == gameObject)
                continue;

            Debug.Log("Hit: " + hit.collider.gameObject.name 
                + " at distance " + hit.distance);

            // Spawn hit effect
            if (hitEffect != null)
                Instantiate(hitEffect, hit.point,
                    Quaternion.LookRotation(hit.normal));

            // Check for zombie on hit object or any parent
            ZombieAI zombie = hit.collider.GetComponentInParent<ZombieAI>();
            if (zombie != null)
            {
                Debug.Log("Zombie killed!");
                zombie.Die();
            }

            // Stop at first valid hit
            break;
        }
    }
}
