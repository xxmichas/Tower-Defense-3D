using UnityEngine;

public class TopView : MonoBehaviour
{
    public GameObject PlayerCam;
    public GameObject TopViewCam;

    private Camera PlayerCamera;
    private Camera TopViewCamera;

    private AudioListener PlayerListener;
    private AudioListener TopViewListener;

    // Start is called before the first frame update
    void Start()
    {
        PlayerCamera = PlayerCam.GetComponent<Camera>();
        TopViewCamera = TopViewCam.GetComponent<Camera>();

        PlayerListener = PlayerCam.GetComponent<AudioListener>();
        TopViewListener = TopViewCam.GetComponent<AudioListener>();

        TopViewCamera.enabled = false;
        TopViewListener.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("SwitchCamera", "C"))))
        {
            PlayerCamera.enabled = !PlayerCamera.enabled;
            PlayerListener.enabled = !PlayerListener.enabled;

            TopViewCamera.enabled = !TopViewCamera.enabled;
            TopViewListener.enabled = !TopViewListener.enabled;
        }
    }
}
