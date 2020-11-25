using UnityEngine;

public class Tower2 : MonoBehaviour
{
    private Transform target;
    private float TargetingSpeed = 20f;

    //Tower Stats
    public float range = 2f;
    public float AttackSpeed = 1f;
    private float AttackCooldown = 0f;

    public GameObject CannonBallPrefab;
    public Transform CannonBallSpawnPoint;

    public AudioSource AttackSound;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        AttackCooldown -= Time.deltaTime;

        if (target == null)
        {
            return;
        }

        Vector3 Dir = target.position - transform.position;
        Quaternion FaceRotation = Quaternion.LookRotation(Dir);
        Vector3 Rotation = Quaternion.Lerp(transform.rotation, FaceRotation, Time.deltaTime * TargetingSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, Rotation.y, 0f);

        if (AttackCooldown <= 0f)
        {
            Attack();
            AttackCooldown = 1f / AttackSpeed;
        }
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
        }
        else
        {
            target = null;
        }
    }

    void Attack()
    {
        GameObject CannonBallObject = (GameObject)Instantiate(CannonBallPrefab, CannonBallSpawnPoint.position, CannonBallSpawnPoint.rotation);

        CannonBall CannonBall = CannonBallObject.GetComponent<CannonBall>();

        AttackSound.Play();

        if (CannonBall != null)
        {
            CannonBall.Seek(target);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
