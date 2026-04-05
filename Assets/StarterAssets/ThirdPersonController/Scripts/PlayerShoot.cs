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
    Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    RaycastHit hit;

    int layerMask = ~LayerMask.GetMask("Player"); 

    // Add 'layerMask' to the end of your Raycast command
    if (Physics.Raycast(ray, out hit, range, layerMask))
    {
        Debug.Log("Hit: " + hit.transform.name);
        
        if (hit.transform.CompareTag("Zombie"))
        {
            Zombie zombieScript = hit.transform.GetComponent<Zombie>();
            if (zombieScript != null)
            {
                zombieScript.TakeDamage(damage);
            }
        }
    }
}
}