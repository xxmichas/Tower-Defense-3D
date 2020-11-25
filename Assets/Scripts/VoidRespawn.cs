using UnityEngine;

public class VoidRespawn : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -25)
        {
            transform.position = new Vector3(0, 4, 0);
        }
    }
}
