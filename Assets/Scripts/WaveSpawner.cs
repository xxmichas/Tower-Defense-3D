using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{    
    public Wave[] Waves;

    [Header("Connect to appropriate GameObjects")]
    public Transform SpawnPoint;

    public Text WaveCountdownText;
    public Text WaveNumberText;

    public Text LevelStartText;

    private int WaveNumber = 0;
    private bool SpawnNextWave;
    private bool LevelStartDelay;

    public static int TotalEnemies;

    void Start()
    {
        TotalEnemies = 0;

        foreach (Wave _Wave in Waves)
        {
            TotalEnemies += _Wave.Count;
            TotalEnemies += _Wave.TotalNumberAdjustment;
        }

        WaveNumberText.text = "Wave: " + WaveNumber.ToString() + "/" + Waves.Length.ToString();

        SpawnNextWave = true;
        LevelStartDelay = false;

        LevelStartText.gameObject.SetActive(true);

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            LevelStartDelay = true;
        }
    }

    void Update()
    {
        if (WaveNumber == Waves.Length && TotalEnemies <= 0)
        {
            GameManager.Victory = true;
            this.enabled = false;
        }
        if (SpawnNextWave && LevelStartDelay && WaveNumber != Waves.Length)
        {
            StartCoroutine(SpawnWave());
            return;
        }

        WaveCountdownText.text = "Enemies Remaining: " + TotalEnemies.ToString();

        if (Input.GetKeyDown(KeyCode.G) && !LevelStartDelay)
        {
            LevelStartDelay = true;
            LevelStartText.gameObject.SetActive(false);
        }
    }

    IEnumerator SpawnWave()
    {
        SpawnNextWave = false;

        Wave Wave = Waves[WaveNumber];

        WaveNumberText.text = "Wave: " + (WaveNumber + 1).ToString() + "/" + Waves.Length.ToString();

        for (int i = 0; i < Wave.Count; i++)
        {
            SpawnEnemy(Wave.Enemy);

            if (i == Wave.Count - 1)
            {
                continue;
            }

            yield return new WaitForSeconds(Wave.SpawnInterval);
        }

        WaveNumber++;
        yield return new WaitForSeconds(Wave.NextWaveDelay);
        SpawnNextWave = true;
    }

    void SpawnEnemy(GameObject Enemy)
    {
        Instantiate(Enemy, SpawnPoint.position, SpawnPoint.rotation);
    }
}
