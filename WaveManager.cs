using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Enemy setup")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    [Header("Wave settings")]
    public int startingEnemies = 2;
    public int enemiesPerWaveIncrease = 2;
    public float timeBetweenWaves = 5f;   // how long to wait before a wave can be started

    [Header("UI")]
    public TMP_Text waveCountdownText;    // text like "Next wave in: 3.0s"
    public Button startWaveButton;        // your "Start Wave" button

    private int currentWave = 0;
    private int enemiesAlive = 0;

    private float countdownTimer = 0f;
    private bool waveInProgress = false;
    private bool canStartWave = false;    // only true when countdown finished

    private void OnEnable()
    {
        EnemyHealth.OnMonsterKilled += HandleEnemyKilled;
    }

    private void OnDisable()
    {
        EnemyHealth.OnMonsterKilled -= HandleEnemyKilled;
    }

    private void Start()
    {
        // No wave at the start – set up first countdown
        BeginInterWaveCountdown();
    }

    private void Update()
    {
        // Only count down when no wave is running and we haven't finished the timer
        if (!waveInProgress && countdownTimer > 0f)
        {
            countdownTimer -= Time.deltaTime;
            if (countdownTimer < 0f) countdownTimer = 0f;

            if (waveCountdownText != null)
                waveCountdownText.text = $"Next wave in: {countdownTimer:0.0}s";

            // When timer hits 0, unlock the button, BUT DO NOT START THE WAVE
            if (countdownTimer <= 0f)
            {
                canStartWave = true;

                if (waveCountdownText != null)
                    waveCountdownText.text = "Next wave ready";

                if (startWaveButton != null)
                    startWaveButton.interactable = true;
            }
        }
    }

    // Hook this to the Start Wave button OnClick()
    public void OnStartWaveButtonPressed()
    {
        if (!canStartWave || waveInProgress)
            return;

        StartNextWave();
    }

    private void BeginInterWaveCountdown()
    {
        waveInProgress = false;
        canStartWave = false;
        countdownTimer = timeBetweenWaves;

        if (startWaveButton != null)
            startWaveButton.interactable = false;

        if (waveCountdownText != null)
            waveCountdownText.text = $"Next wave in: {countdownTimer:0.0}s";
    }

    private void StartNextWave()
    {
        waveInProgress = true;
        canStartWave = false;

        if (startWaveButton != null)
            startWaveButton.interactable = false;

        if (waveCountdownText != null)
            waveCountdownText.text = "";

        currentWave++;
        int enemiesThisWave = startingEnemies + (currentWave - 1) * enemiesPerWaveIncrease;
        enemiesAlive = enemiesThisWave;

        SpawnWave(enemiesThisWave);
        Debug.Log($"Wave {currentWave} started. Spawning {enemiesThisWave} enemies.");
    }

    private void SpawnWave(int count)
    {
        if (enemyPrefab == null || spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("WaveManager: set enemyPrefab and spawnPoints in Inspector.");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemyPrefab, spawn.position, Quaternion.identity);
        }
    }

    private void HandleEnemyKilled(int expGained)
    {
        if (!waveInProgress) return;

        enemiesAlive--;

        if (enemiesAlive <= 0)
        {
            // wave finished – start a new countdown, but DO NOT start next wave
            BeginInterWaveCountdown();
        }
    }
}
