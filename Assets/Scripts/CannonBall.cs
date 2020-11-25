using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private Transform target;

    public GameObject Explosion;

    //Projectile Stats
    public float speed = 15f;
    public int DamageAmount = 35;
    public float ExplosionRadius = 1.5f;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 Dir = target.position - transform.position;
        float DistanceThisFrame = speed * Time.deltaTime;

        if (Dir.magnitude <= DistanceThisFrame)
        {
            Explode();
            return;
        }

        transform.Translate(Dir.normalized * DistanceThisFrame, Space.World);
    }

    void Explode()
    {
        Instantiate(Explosion, transform.position, transform.rotation);

        Collider[] ObjectsInRange =  Physics.OverlapSphere(transform.position, ExplosionRadius);
        foreach (Collider Object in ObjectsInRange)
        {
            if (Object.tag == "Enemy")
            {
                Damage(Object.transform);
            }
        }
        Destroy(gameObject);
    }

    void Damage(Transform EnemyObject)
    {
        Enemy TargetEnemy = EnemyObject.GetComponent<Enemy>();
        TargetEnemy.OnDamageTaken(DamageAmount);


        Destroy(gameObject);
    }
}
