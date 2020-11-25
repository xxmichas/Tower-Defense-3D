using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
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

    public Transform EnemiesParent;

    private bool StartTutorial;
    private bool FirstSlide;

    [SerializeField]
    private CanvasGroup TutorialObject;
    public GameObject[] TutorialSlides;
    public GameObject[] ObjectsToHideDuringSlide;
    private int LastSlide = 1;

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
        FirstSlide = true;

        LevelStartText.gameObject.SetActive(true);

        foreach (GameObject Slide in TutorialSlides)
        {
            Slide.SetActive(false);
        }

        TutorialSlides[0].SetActive(true);

        foreach (GameObject Obj in ObjectsToHideDuringSlide)
        {
            Obj.SetActive(false);
        }
    }

    void Update()
    {
        if (GameManager.GameIsPaused)
        {
            TutorialObject.alpha = 0;
        }
        else
        {
            TutorialObject.alpha = 1;
        }

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

        if (Input.GetKeyDown(KeyCode.G) && !LevelStartDelay && EnemiesParent.childCount == 0)
        {
            if (FirstSlide)
            {
                TutorialSlides[0].SetActive(false);
                FirstSlide = false;

                TutorialSlides[1].SetActive(true);
            }
            else
            {
                LevelStartDelay = true;
                StartTutorial = false;
                LevelStartText.gameObject.SetActive(false);

                if (LastSlide == TutorialSlides.Length)
                {
                    
                }
                else
                {
                    TutorialSlides[LastSlide].SetActive(false);
                    LastSlide++;
                }

                foreach (GameObject Obj in ObjectsToHideDuringSlide)
                {
                    Obj.SetActive(true);
                }
            }
        }

        if (SpawnNextWave && EnemiesParent.childCount == 0 && StartTutorial)
        {
            LevelStartText.gameObject.SetActive(true);

            ShowSlide();
        }
    }

    private void ShowSlide()
    {
        if (LastSlide == TutorialSlides.Length)
        {
            return;
        }
        TutorialSlides[LastSlide].SetActive(true);

        foreach (GameObject Obj in ObjectsToHideDuringSlide)
        {
            Obj.SetActive(false);
        }
    }

    IEnumerator SpawnWave()
    {
        SpawnNextWave = false;

        LevelStartDelay = false;

        StartTutorial = true;

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
        SpawnNextWave = true;
    }

    void SpawnEnemy(GameObject Enemy)
    {
        Instantiate(Enemy, SpawnPoint.position, SpawnPoint.rotation, EnemiesParent);
    }
}
