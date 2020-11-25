using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject Canvas;
    private bool FirstScreen = true;

    public GameObject Camera;
    private Vector3 CameraInitialPosition = new Vector3();
    private Quaternion targetRotation;

    public GameObject SelectWaypoint;
    private Vector3 Waypoint = new Vector3();

    public CanvasGroup SkyCanvasGroup;
    public Animation CanvasFadeIn;
    private bool OnSkyCanvas;

    private bool OnSignCanvas;

    private int LevelsUnlocked;
    public GameObject Content;
    private GameObject TempChild;

    Resolution[] Resolutions;
    public TMPro.TMP_Dropdown ResolutionsDropdown;
    public Toggle FullScreen;

    public Slider VolumeSlider;
    public AudioMixer Master;
    public TMPro.TMP_Text VolumeSliderText;

    public Slider SensitivitySlider;
    public TMPro.TMP_Text SensitivitySliderText;

    public GameObject ExitPopUp;
    public GameObject ExitConfirmBox;

    AsyncOperation LoadingScene;
    public Animation LoadingScreenFadeIn;
    public Image LoadingBar;
    private bool LoadingNextScene = false;

    void Start()
    {
        Time.timeScale = 1f;

        CameraInitialPosition = Camera.transform.position;
        targetRotation = Camera.transform.rotation;
        Waypoint = Camera.transform.position;

        SkyCanvasGroup.alpha = 0f;
        SkyCanvasGroup.blocksRaycasts = false;
        OnSkyCanvas = false;

        OnSignCanvas = false;

        LevelsUnlocked = PlayerPrefs.GetInt("Lvl", 1);

        for (int i = 0; i < LevelsUnlocked; i++)
        {
            TempChild = Content.transform.GetChild(i).gameObject;

            if (i == LevelsUnlocked - 1)
            {

            }
            else
            {
                TempChild.GetComponent<Image>().color = new Color32(85, 255, 150, 255);
            }
            TempChild.GetComponent<Button>().interactable = true;
            TempChild.transform.Find("Locked").gameObject.SetActive(false);
            TempChild.transform.Find("Text (TMP)").gameObject.SetActive(true);
        }

        Resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();

        ResolutionsDropdown.ClearOptions();

        List<string> options = new List<string>();

        int CurrentResolutionIndex = 0;

        for (int i = 0; i < Resolutions.Length; i++)
        {
            string option = Resolutions[i].width + " x " + Resolutions[i].height;
            options.Add(option);

            if (Resolutions[i].width == Screen.width && Resolutions[i].height == Screen.height)
            {
                CurrentResolutionIndex = i;
            }
        }

        ResolutionsDropdown.AddOptions(options);
        ResolutionsDropdown.value = CurrentResolutionIndex;
        ResolutionsDropdown.RefreshShownValue();

        if (Screen.fullScreen)
        {
            FullScreen.isOn = true;
        }
        else
        {
            FullScreen.isOn = false;
        }

        Master.SetFloat("Volume", PlayerPrefs.GetFloat("Volume", 0f));
        VolumeSlider.value = PlayerPrefs.GetFloat("Volume", 0f);

        SensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", 2f);

        ExitPopUp.SetActive(false);
        ExitConfirmBox.SetActive(false);
    }

    void Update()
    {
        if (!LoadingNextScene)
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown("escape"))
                {
                    if (OnSignCanvas)
                    {
                        BackToStartScreen();
                    }
                    else if (OnSkyCanvas)
                    {
                        StopAllCoroutines();
                        Back();
                    }
                    else if (FirstScreen)
                    {
                        ExitWindow();
                    }
                }
                else
                {
                    if (FirstScreen)
                    {
                        Canvas.SetActive(false);
                        FirstScreen = false;
                        StartScreen();
                    }
                }
            }
        }

        Camera.transform.rotation = Quaternion.Lerp(Camera.transform.rotation, targetRotation, 3 * 1f * Time.deltaTime);
        Camera.transform.position = Vector3.Lerp(Camera.transform.position, Waypoint, 3 * 1f * Time.deltaTime);
    }

    public void ExitWindow()
    {
        ExitPopUp.SetActive(true);
        StartCoroutine(ExitPopUpAnimationDelay());

        FirstScreen = false;
    }

    IEnumerator ExitPopUpAnimationDelay()
    {
        yield return new WaitForSeconds(0.5f);
        ExitPopUp.GetComponent<Animation>().Stop();
        ExitConfirmBox.SetActive(true);
    }

    public void ExitWindowNo()
    {
        ExitPopUp.SetActive(false);
        ExitConfirmBox.SetActive(false);

        FirstScreen = true;
    }

    public void BackToStartScreen()
    {
        Canvas.SetActive(true);
        FirstScreen = true;
        OnSignCanvas = false;
        targetRotation = Quaternion.AngleAxis(180, Vector3.up);
        Waypoint = CameraInitialPosition;
    }

    public void Play()
    {
        targetRotation = Quaternion.Euler(-90, 270, 0);

        CanvasFadeIn.Play();
        OnSkyCanvas = true;

        OnSignCanvas = false;

        StartCoroutine(ButtonsFadeIn());
    }

    IEnumerator ButtonsFadeIn()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < ((Content.transform.childCount / 5) + 1); i++)
        {
            StartCoroutine(SequentialFadeIn((i * 5), ((i * 5)+5)));

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator SequentialFadeIn(int Start, int End)
    {
        if (End > Content.transform.childCount)
        {
            End = Content.transform.childCount;
        }

        for (int i = Start; i < End; i++)
        {
            Content.transform.GetChild(i).gameObject.GetComponent<Animation>().Play();

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Back()
    {
        CanvasFadeIn.Play("CanvasFadeOut");
        StartScreen();
        OnSkyCanvas = false;

        for (int i = 0; i < Content.transform.childCount; i++)
        {
            TempChild = Content.transform.GetChild(i).gameObject;

            TempChild.GetComponent<Animation>().Stop();
            TempChild.GetComponent<CanvasGroup>().alpha = 0;
            TempChild.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void StartLevel(int LevelIndex)
    {
        if (LoadingNextScene)
        {
            return;
        }
        else
        {
            LoadingNextScene = true;

            CanvasFadeIn.Play("CanvasFadeOut");
            LoadingScreenFadeIn.Play();

            StartCoroutine(LoadScene(LevelIndex));
        }
    }

    IEnumerator LoadScene(int Index)
    {
        bool LoadOnce = true;

        yield return new WaitForSeconds(1f);

        LoadingScene = SceneManager.LoadSceneAsync(Index);
        LoadingScene.allowSceneActivation = false;

        while (LoadingScene.isDone == false)
        {
            print(LoadingScene.progress);
            LoadingBar.fillAmount = LoadingScene.progress / 0.9f;

            if (LoadingScene.progress >= 0.9f && LoadOnce)
            {
                LoadOnce = false;

                LoadingScreenFadeIn.Play("CanvasFadeOut");
                yield return new WaitForSeconds(1f);
                LoadingScene.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void StartScreen()
    {
        targetRotation = Quaternion.AngleAxis(270, Vector3.up);
        Waypoint = SelectWaypoint.transform.position;

        OnSignCanvas = true;
    }

    public void SetResolution(int ResolutionIndex)
    {
        Resolution resolution = Resolutions[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void Fullscreen(bool Fullscreen)
    {
        Screen.fullScreen = Fullscreen;
    }

    public void SetVolume(float Volume)
    {
        Master.SetFloat("Volume", Volume);
        PlayerPrefs.SetFloat("Volume", Volume);
        VolumeSliderText.text = Mathf.Round((Volume / 80 * 100) + 100).ToString();
    }

    public void SetSensitivity(float Sensitivity)
    {
        PlayerPrefs.SetFloat("Sensitivity", Sensitivity);
        SensitivitySliderText.text = String.Format("{0:0.00}", Sensitivity).Replace(",", ".");
    }
}