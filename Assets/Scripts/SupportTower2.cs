using System.Collections;
using UnityEngine;

public class SupportTower2 : MonoBehaviour
{
    public int MoneyBonus = 1;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayedStart());
    }

    private void OnDestroy()
    {
        Stats.MoneyBonus = 0;
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(2f);
        Stats.MoneyBonus = MoneyBonus;
    }
}