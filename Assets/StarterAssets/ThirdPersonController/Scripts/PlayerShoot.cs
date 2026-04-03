using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Settings")]
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 0.5f;
    
    [Header("References")]
    public Camera mainCamera;
    
    private float nextTimeToFire = 0f;

    void Update()
    {
        // Check if player clicks Left Mouse Button and if enough time has passed
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // Create a ray from the center of the viewport (the screen)
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Perform the Raycast
        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);

            // Check if the object we hit has the "Zombie" tag
            if (hit.transform.CompareTag("Zombie"))
            {
                // Try to find a script on the zombie that has TakeDamage
                // Replace 'ZombieHealth' with whatever your health script name is
                var zombie = hit.transform.GetComponent<ZombieHealth>();
                if (zombie != null)
                {
                    zombie.TakeDamage(damage);
                }
            }
        }
    }
}