using UnityEngine;
using TMPro; // Needed for the countdown UI
using System.Collections; // Needed for the Coroutine (pause)

public class ZombieSpawner : MonoBehaviour
{
    public GameObject[] zombiePrefabs;
    public Transform[] spawnPoints;
    public float spawnInterval = 3f;
    private float timer;

    [Header("Difficulty & Waves")]
    public int zombiesPerWave = 10;
    public float speedBoostPerWave = 0.5f;
    public float wavePauseDuration = 5f; // Duration of the pause (Item #8)

    [Header("UI References")]
    public TextMeshProUGUI countdownText; // Drag your 'Next Wave' text here

    private int waveNumber = 1;
    private int zombiesSpawnedInWave = 0;
    private bool isPaused = false; // Prevents spawning during the break

    void Start()
    {
        timer = spawnInterval;
        
        // Hide countdown text at the start
        if (countdownText != null) 
            countdownText.gameObject.SetActive(false);

        // Tell UI we are starting Wave 1
        if (ScoreManager.instance != null) 
            ScoreManager.instance.UpdateWaveUI(waveNumber);
    }

    void Update()
    {
        // Don't count down the spawn timer if we are in a wave pause
        if (isPaused) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnZombie();
            timer = spawnInterval;
        }
    }

    void SpawnZombie()
    {
        if (spawnPoints.Length == 0 || zombiePrefabs.Length == 0) return;

        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];

        int zombieIndex = Random.Range(0, zombiePrefabs.Length);
        GameObject prefabToSpawn = zombiePrefabs[zombieIndex];

        GameObject newZombie = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);

        // Apply speed boost (Item #3)
        UnityEngine.AI.NavMeshAgent agent = newZombie.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.speed += (waveNumber * speedBoostPerWave);
        }

        zombiesSpawnedInWave++;

        // Check if wave is finished
        if (zombiesSpawnedInWave >= zombiesPerWave)
        {
            StartCoroutine(WavePauseRoutine());
        }
    }

    // This handles the delay and countdown between waves (Item #8)
    IEnumerator WavePauseRoutine()
    {
        isPaused = true;
        zombiesSpawnedInWave = 0;

        // Show the countdown UI
        if (countdownText != null) 
            countdownText.gameObject.SetActive(true);

        float pauseTimer = wavePauseDuration;

        while (pauseTimer > 0)
        {
            if (countdownText != null) 
                countdownText.text = "Next Wave in: " + Mathf.Ceil(pauseTimer).ToString();
            
            yield return new WaitForSeconds(1f);
            pauseTimer--;
        }

        // Hide UI and update wave stats
        if (countdownText != null) 
            countdownText.gameObject.SetActive(false);
        
        waveNumber++;
        spawnInterval = Mathf.Max(0.5f, spawnInterval - 0.2f); // Make spawning faster

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.UpdateWaveUI(waveNumber);
        }
        
        Debug.Log("Wave " + waveNumber + " Started!");
        isPaused = false;
    }
}