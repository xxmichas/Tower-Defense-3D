using System.Collections;
using UnityEngine;

public class SupportTower1 : MonoBehaviour
{
    private float BaseCooldown;
    public float CooldownReduction = 0.7f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayedStart());
    }
    private void OnDestroy()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PickupObject>().LevelCooldown = BaseCooldown;
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(2f);
        BaseCooldown = GameObject.FindGameObjectWithTag("Player").GetComponent<PickupObject>().LevelCooldown;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PickupObject>().LevelCooldown *= CooldownReduction;
    }
}
