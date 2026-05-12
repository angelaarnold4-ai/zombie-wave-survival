using UnityEngine;
using TMPro;
using System.Collections;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject[] zombiePrefabs;
    public Transform[] spawnPoints;
    public float spawnInterval = 3f;
    private float timer;

    [Header("Difficulty & Waves")]
    public int zombiesPerWave = 10;
    public float speedBoostPerWave = 0.5f;
    public float wavePauseDuration = 5f;

    [Header("UI References")]
    public TextMeshProUGUI countdownText;

    private int waveNumber = 1;
    private int zombiesSpawnedInWave = 0;
    private bool isPaused = false;

    // FIX 1: Synchronous guard — set immediately when the wave limit is hit,
    // before the coroutine even starts. This stops SpawnZombie() from being
    // called again (and starting duplicate coroutines) on the next Update tick.
    private bool isWaveComplete = false;

    void Start()
    {
        timer = spawnInterval;

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        if (ScoreManager.instance != null)
            ScoreManager.instance.UpdateWaveUI(waveNumber);
    }

    void Update()
    {
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
        // FIX 1: Bail out immediately if the wave is already done.
        // Prevents multiple WavePauseRoutine coroutines from stacking up.
        if (isWaveComplete) return;

        if (spawnPoints.Length == 0 || zombiePrefabs.Length == 0) return;

        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];

        int zombieIndex = Random.Range(0, zombiePrefabs.Length);
        GameObject prefabToSpawn = zombiePrefabs[zombieIndex];

        GameObject newZombie = Instantiate(
            prefabToSpawn, spawnPoint.position, spawnPoint.rotation);

        UnityEngine.AI.NavMeshAgent agent =
            newZombie.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
            agent.speed += (waveNumber * speedBoostPerWave);

        zombiesSpawnedInWave++;

        if (zombiesSpawnedInWave >= zombiesPerWave)
        {
            // FIX 1: Set the guard synchronously RIGHT HERE, before yielding
            // control back to Update. The coroutine sets isPaused a frame later —
            // this flag plugs that gap.
            isWaveComplete = true;
            StartCoroutine(WavePauseRoutine());
        }
    }

    IEnumerator WavePauseRoutine()
    {
        isPaused = true;
        zombiesSpawnedInWave = 0;

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

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        waveNumber++;
        spawnInterval = Mathf.Max(0.5f, spawnInterval - 0.2f);

        if (ScoreManager.instance != null)
            ScoreManager.instance.UpdateWaveUI(waveNumber);

        Debug.Log("Wave " + waveNumber + " Started!");

        // FIX 1: Clear BOTH flags so spawning resumes cleanly for the new wave.
        isWaveComplete = false;
        isPaused = false;
    }
}
