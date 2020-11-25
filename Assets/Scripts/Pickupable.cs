using UnityEngine;

public class Pickupable : MonoBehaviour
{
	public float Cooldown = 0f;

	public string TowerType = "Tower Name";
	
	// Update is called once per frame
	void Update () {
		if (Cooldown > 0f)
		{
			Cooldown -= Time.deltaTime;
		}
		if (Cooldown < 0f)
		{
			Cooldown = 0f;
		}
	}
}