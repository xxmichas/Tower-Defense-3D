using UnityEngine;

public class MouseLook1 : MonoBehaviour
{
    float Sensitivity = 250f;

    public Transform Player;

    public static bool PauseCameraMovement;

    float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        PauseCameraMovement = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseCameraMovement)
        {
            float mouseX = Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * Sensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            Player.Rotate(Vector3.up * mouseX);
        }
    }
}
