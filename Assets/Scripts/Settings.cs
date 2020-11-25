using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    Resolution[] Resolutions;
    public TMPro.TMP_Dropdown ResolutionsDropdown;
    public Toggle FullScreen;

    public Slider VolumeSlider;
    public AudioMixer Master;
    public TMPro.TMP_Text VolumeSliderText;

    public Slider SensitivitySlider;
    public TMPro.TMP_Text SensitivitySliderText;

    public MouseLook MouseSensitivity;

    private Event KeyEvent;
    private KeyCode NewKey;
    private bool WaitingForKey;
    private TMPro.TMP_Text ButtonText;

    public TMPro.TMP_Text JumpText;
    public TMPro.TMP_Text ShopText;
    public TMPro.TMP_Text TowerMenuText;
    public TMPro.TMP_Text UIText;
    public TMPro.TMP_Text TowerRangeText;
    public TMPro.TMP_Text ViewText;

    private void Start()
    {
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

        MouseSensitivity.XSensitivity = Sensitivity;
        MouseSensitivity.YSensitivity = Sensitivity;
    }
    
    void OnGUI()
    {
        KeyEvent = Event.current;

        if (KeyEvent.isKey && WaitingForKey)
        {
            NewKey = KeyEvent.keyCode;

            WaitingForKey = false;
        }
    }

    public void StartAssignment(string KeyName)
    {
        if (!WaitingForKey)
        {
            StartCoroutine(AssignKey(KeyName));
        }
    }

    public void SendText(TMPro.TMP_Text text)
    {
        ButtonText = text;
        ButtonText.text = "...";
    }

    IEnumerator WaitForKey()
    {
        while (!KeyEvent.isKey)
        {
            yield return null;
        }
    }

    public IEnumerator AssignKey(string KeyName)
    {
        WaitingForKey = true;

        yield return WaitForKey();

        ButtonText.text = NewKey.ToString();
        PlayerPrefs.SetString(KeyName, NewKey.ToString());
        
        yield return null;
    }

    private void OnEnable()
    {
        JumpText.text = PlayerPrefs.GetString("Jump", "Space");
        ShopText.text = PlayerPrefs.GetString("ShopKey", "E");
        TowerMenuText.text = PlayerPrefs.GetString("TowerMenuKey", "R");
        UIText.text = PlayerPrefs.GetString("HideUIKey", "U");
        TowerRangeText.text = PlayerPrefs.GetString("GizmosKey", "F");
        ViewText.text = PlayerPrefs.GetString("SwitchCamera", "C");
    }
}