using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool Victory;
    private bool OnlyWinOnce;
    public static bool GameOver;
    private bool OnlyLoseOnce;

    public GameObject VictoryUI;
    public GameObject GameOverUI;

    public GameObject ShopMenu;
    public GameObject TowerMenu;

    public GameObject PauseMenu;
    public static bool GameIsPaused;

    public GameObject UI;

    public static AudioSource TentDamageSound;
    public AudioSource WinSound;
    public AudioSource LoseSound;

    public static Animation LivesAnimation;

    public GameObject Towers;
    private List<Transform> TowerObjects = new List<Transform>();
    private bool Gizmos;

    void Start()
    {
        Victory = false;
        OnlyWinOnce = true;
        GameOver = false;
        OnlyLoseOnce = true;

        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
        GameIsPaused = false;

        TentDamageSound = gameObject.GetComponent<AudioSource>();

        LivesAnimation = GameObject.Find("Lives").GetComponent<Animation>();

        Gizmos = false;
    }
    void Update()
    {
        if (GameOver && OnlyLoseOnce)
        {
            LevelLost();
            OnlyLoseOnce = false;
            OnlyWinOnce = false;
        }
        else
        {
            if (Victory && OnlyWinOnce)
            {
                LevelWon();
                OnlyWinOnce = false;
                OnlyLoseOnce = false;
            }
        }
        if (!Victory && !GameOver)
        {
            if (Input.GetKeyDown(PlayerPrefs.GetString("PauseKey", "escape")))
            {
                if (!PauseMenu.activeSelf)
                {
                    PauseGame();
                }
                else
                {
                    ResumeGame();
                }
            }

            if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("HideUIKey", "U"))))
            {
                UI.SetActive(!UI.activeSelf);
            }

            if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("GizmosKey", "F"))))
            {
                ShowGizmos();
            }
        }
    }

    void LevelWon()
    {
        VictoryUI.SetActive(true);
        UI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        ShopMenu.SetActive(false);
        TowerMenu.SetActive(false);

        WinSound.Play();

        if (SceneManager.sceneCountInBuildSettings == (SceneManager.GetActiveScene().buildIndex + 1))
        {

        }
        else
        {
            PlayerPrefs.SetInt("Lvl", (SceneManager.GetActiveScene().buildIndex + 1));
        }
    }

    void LevelLost()
    {
        GameOverUI.SetActive(true);
        UI.SetActive(true);

        ShopMenu.SetActive(false);
        TowerMenu.SetActive(false);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        LoseSound.Play();
    }

    private void PauseGame()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        MouseLook.PauseCameraMovement = true;
    }

    private void ResumeGame()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        MouseLook.PauseCameraMovement = false;
    }

    public static void DamageSound()
    {
        TentDamageSound.Play();

        LivesAnimation.Stop();
        LivesAnimation.Play();
    }

    private void ShowGizmos()
    {
        Gizmos = !Gizmos;
        TowerObjects.Clear();
        TowerObjects = Towers.transform.Cast<Transform>().ToList();
        foreach (Transform Tower in TowerObjects)
        {
            Tower.Find("Gizmo").gameObject.SetActive(Gizmos);
        }
    }

    public void NextLevel()
    {
        if (SceneManager.sceneCountInBuildSettings == (SceneManager.GetActiveScene().buildIndex + 1))
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}