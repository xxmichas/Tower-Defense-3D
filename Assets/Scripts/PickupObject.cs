using System;
using UnityEngine;
using UnityEngine.UI;

public class PickupObject : MonoBehaviour
{
	public GameObject mainCamera;
	bool carrying;

	private Transform carriedObject;
	private Pickupable p;

	public float distance;
	public float smooth;

	private float MaxDistance = 3.5f;
	public float LevelCooldown = 15f;

	public Transform PopUpLocation;
	public GameObject CooldownPopUp;

	// Update is called once per frame
	void Update () {
		if(carrying) {
			carry(carriedObject);
			checkDrop();
		} else {
			pickup();
		}
	}
	
	void carry(Transform o) {
		o.position = Vector3.Lerp (o.position, mainCamera.transform.position + mainCamera.transform.forward * distance, smooth);
		o.rotation = transform.rotation;
	}
	
	void pickup() {
		if(Input.GetMouseButtonDown(1)) {
			int x = Screen.width / 2;
			int y = Screen.height / 2;
			
			Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x,y));
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit, 1 << 10) && (hit.distance <= MaxDistance)) {
				p = hit.collider.GetComponent<Pickupable>();
				if(p != null) {
					if (p.Cooldown > 0f)
					{
						GameObject PopUp = Instantiate(CooldownPopUp, PopUpLocation.position, PopUpLocation.rotation, PopUpLocation);
						Text PopUpText = PopUp.transform.Find("Text").GetComponent<Text>();
						PopUpText.text = "You can't pick up this tower yet!\nWait "+ Math.Round(p.Cooldown, 1) + "s";
					}
					else
					{
						carrying = true;
						carriedObject = p.transform;
						p.gameObject.GetComponent<Rigidbody>().isKinematic = true;
						var Colliders = p.gameObject.GetComponents<Collider>();
						foreach (var Collider in Colliders)
						{
							Collider.enabled = false;
						}

						if (p.TowerType != "Global")
                        {
							MonoBehaviour TowerScript = p.GetComponent(p.TowerType) as MonoBehaviour;
							TowerScript.enabled = false;
						}
					}
				}
			}
		}
	}
	
	void checkDrop() {
		if(Input.GetMouseButtonDown(1)) {
			dropObject();
		}
	}
	
	void dropObject() {
		carrying = false;
		var Colliders = carriedObject.GetComponents<Collider>();
		foreach (var Collider in Colliders)
		{
			Collider.enabled = true;
		}

		if (p.TowerType != "Global")
        {
			MonoBehaviour TowerScript = carriedObject.GetComponent(p.TowerType) as MonoBehaviour;
			TowerScript.enabled = true;
		}

		carriedObject.GetComponent<Pickupable>().Cooldown = LevelCooldown;

		carriedObject.GetComponent<Rigidbody>().isKinematic = false;
		carriedObject = null;
		p = null;
	}
}

