using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public GameObject zombiePrefab; 
    public float spawnRadius = 20f; // Distance from center
    public float timeBetweenWaves = 5f;
    
    private int currentWave = 0;
    private int zombiesToSpawn;
    private int zombiesRemaining;

    void Start()
    {
        StartNextWave();
    }

    void StartNextWave()
    {
        currentWave++;
        zombiesToSpawn = currentWave * 5; // Wave 1 = 5, Wave 2 = 10, etc.
        zombiesRemaining = zombiesToSpawn;

        if(ScoreManager.instance != null)
            ScoreManager.instance.UpdateWaveUI(currentWave);

        StartCoroutine(SpawnWaveRoutine());
    }

    IEnumerator SpawnWaveRoutine()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        for (int i = 0; i < zombiesToSpawn; i++)
        {
            // Perimeter Spawning: Pick a random spot on a circle
            Vector2 randomPoint = Random.insideUnitCircle.normalized * spawnRadius;
            Vector3 spawnPos = new Vector3(randomPoint.x, 0, randomPoint.y);

            Instantiate(zombiePrefab, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(0.5f); 
        }
    }

    public void ZombieDied()
    {
        zombiesRemaining--;
        ScoreManager.instance.AddKill();

        if (zombiesRemaining <= 0)
        {
            StartNextWave();
        }
    }
}