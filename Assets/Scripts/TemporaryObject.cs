using UnityEngine;

public class TemporaryObject : MonoBehaviour
{
    //Set this value in Unity Editor
    public float Delay = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, Delay);
    }
}
