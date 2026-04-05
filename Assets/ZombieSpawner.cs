using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 3f;
    private float timer;

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnZombie();
            timer = spawnInterval;
        }
    }

    void SpawnZombie()
    {
        if (spawnPoints.Length == 0) return;

        int index = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[index];

        Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);
    }
}