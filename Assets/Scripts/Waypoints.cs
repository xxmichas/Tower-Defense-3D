using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] WaypointsArray;

    void Awake()
    {
        WaypointsArray = new Transform[transform.childCount];

        for (int i = 0; i < WaypointsArray.Length; i++)
        {
            WaypointsArray[i] = transform.GetChild(i);
        }
    }
}
