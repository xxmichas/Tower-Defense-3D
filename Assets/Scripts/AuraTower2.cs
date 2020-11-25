using System.Collections.Generic;
using UnityEngine;

public class AuraTower2 : MonoBehaviour
{
    private List<Transform> targets = new List<Transform>();

    //Tower Stats
    public float range = 3f;
    public float SlowAmount = 0.5f;

    private Enemy EnemyTarget;

    private void OnEnable()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.3f);
    }

    void UpdateTarget ()
    {
        if (targets.Count != 0)
        {
            foreach (Transform target in targets)
            {
                try
                {
                    EnemyTarget = target.GetComponent<Enemy>();
                    EnemyTarget.UnSlow();
                }
                catch (System.Exception)
                {

                }
            }
            targets.Clear();
        }


        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");

        int NumberofEnemiesInRange = -1;

        foreach (GameObject Enemy in Enemies)
        {
            float EnemyDistance = Vector3.Distance(transform.position, Enemy.transform.position);
            if (EnemyDistance < range)
            {
                NumberofEnemiesInRange++;
                targets.Add(Enemy.transform);
            }
        }

        if (NumberofEnemiesInRange <= -1)
        {
            targets.Clear();
        }
        else
        {
            foreach (Transform target in targets)
            {
                EnemyTarget = target.GetComponent<Enemy>();
                Slow(EnemyTarget);
            }
        }
    }

    void Slow(Enemy TargetToAttack)
    {
        TargetToAttack.OnSlow(SlowAmount);
    }

    private void OnDisable()
    {
        CancelInvoke();

        if (targets.Count != 0)
        {
            foreach (Transform target in targets)
            {
                try
                {
                    EnemyTarget = target.GetComponent<Enemy>();
                    EnemyTarget.UnSlow();
                }
                catch (System.Exception)
                {

                }
            }
            targets.Clear();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
