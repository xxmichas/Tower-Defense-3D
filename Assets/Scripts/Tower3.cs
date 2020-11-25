using UnityEngine;

public class Tower3 : MonoBehaviour
{
    private Transform target;

    //Tower Stats
    public float range = 3f;
    //Wywolywane 50 razy an sekunde
    public float DamageAmount = 2f;

    public Transform LaserSpawnPoint;
    public LineRenderer LaserRenderer;


    private Enemy EnemyTarget;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            if (LaserRenderer.enabled)
            {
                LaserRenderer.enabled = false;
            }

            return;
        }
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            if (LaserRenderer.enabled)
            {
                LaserRenderer.enabled = false;
            }

            return;
        }

        Attack();
    }

    void UpdateTarget ()
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float ClosestDistance = Mathf.Infinity;
        GameObject ClosestEnemy = null;

        foreach (GameObject Enemy in Enemies)
        {
            float EnemyDistance = Vector3.Distance(transform.position, Enemy.transform.position);
            if (EnemyDistance < ClosestDistance)
            {
                ClosestDistance = EnemyDistance;
                ClosestEnemy = Enemy;
            }
        }

        if (ClosestEnemy != null && ClosestDistance <= range)
        {
            target = ClosestEnemy.transform;
            EnemyTarget = ClosestEnemy.GetComponent<Enemy>();
        }
        else
        {
            target = null;
        }
    }

    void Attack()
    {
        EnemyTarget.OnDamageTaken(DamageAmount);

        if (!LaserRenderer.enabled)
        {
            LaserRenderer.enabled = true;
        }

        LaserRenderer.SetPosition(0, LaserSpawnPoint.position);
        LaserRenderer.SetPosition(1, target.position);
    }

    private void OnDisable()
    {
        LaserRenderer.SetPosition(0, new Vector3(0, -30, 0));
        LaserRenderer.SetPosition(1, new Vector3(0, -30, 0));
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
