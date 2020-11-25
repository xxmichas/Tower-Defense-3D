using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;

    public GameObject HitParticle;

    //Projectile Stats
    public float speed = 15f;
    public int DamageAmount = 40;

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
            Damage(target);
            return;
        }

        transform.Translate(Dir.normalized * DistanceThisFrame, Space.World);
    }

    void Damage(Transform EnemyObject)
    {
        Enemy TargetEnemy = EnemyObject.GetComponent<Enemy>();
        TargetEnemy.OnDamageTaken(DamageAmount);

        Instantiate(HitParticle, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
