using UnityEngine;

[System.Serializable]
public class Wave
{
    public GameObject Enemy;
    public int Count;
    public float SpawnInterval;
    public float NextWaveDelay;
    public int TotalNumberAdjustment;
}
