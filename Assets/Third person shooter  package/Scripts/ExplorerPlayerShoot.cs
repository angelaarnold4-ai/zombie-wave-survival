using UnityEngine;
using UnityEngine.InputSystem;

public class ExplorerPlayerShoot : MonoBehaviour
{
    [Header("Shooting")]
    public float damage = 25f;
    public float range = 200f;
    public float fireRate = 0.15f;
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
    }

    void Update()
    {
        if (Mouse.current.leftButton.isPressed)
            TryShoot();
    }

    bool ShouldSkip(Collider col)
    {
        // Skip player's own colliders
        if (col.transform.IsChildOf(transform) || 
            col.gameObject == gameObject)
            return true;

        // Skip triggers — post process volumes, 
        // reflection probes are all triggers
        if (col.isTrigger)
            return true;

        // Skip specific layers
        if (col.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
            return true;

        if (col.gameObject.layer == LayerMask.NameToLayer("UI"))
            return true;

        if (col.gameObject.layer == LayerMask.NameToLayer("Water"))
            return true;

        return false;
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

        Ray ray = _mainCamera.ScreenPointToRay(
            new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

        // Cast against ALL colliders
        RaycastHit[] hits = Physics.SphereCastAll(
            ray.origin,
            sphereCastRadius,
            ray.direction,
            range);

        // Sort by distance
        System.Array.Sort(hits, (a, b) =>
            a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in hits)
        {
            // Skip invalid objects
            if (ShouldSkip(hit.collider))
                continue;

            Debug.Log("Valid hit: " + hit.collider.gameObject.name
                + " layer: " + LayerMask.LayerToName(hit.collider.gameObject.layer)
                + " isTrigger: " + hit.collider.isTrigger);

            // Spawn hit effect
            if (hitEffect != null)
                Instantiate(hitEffect, hit.point,
                    Quaternion.LookRotation(hit.normal));

            // Check for zombie
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
