using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float speed = 5f;
    public int DamageAmount = 50;
    private float range = 0.45f;

    public int MaxTargets = 3;
    private int TargetsHit = 0;

    private Vector3 Direction = new Vector3(0,0,0);
    private Quaternion Rotation = new Quaternion(0,0,0,0);
    private List<Transform> targets = new List<Transform>();

    void Start()
    {
        transform.rotation = Rotation * Quaternion.Euler(0,0, 0);
    }

    void Update()
    {
        transform.Translate(Direction * speed * Time.deltaTime, Space.World);
       
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");


        foreach (GameObject Enemy in Enemies)
        {
            float EnemyDistance = Vector3.Distance(transform.position, Enemy.transform.position);
            if (EnemyDistance < range)
            {
                if (!targets.Contains(Enemy.transform))
                {
                    Damage(Enemy.transform);
                    targets.Add(Enemy.transform);

                    TargetsHit += 1;
                }
            }
        }

        if (TargetsHit >= MaxTargets)
        {
            Destroy(gameObject);
        }
    }
    void Damage(Transform EnemyObject)
    {
        Enemy TargetEnemy = EnemyObject.GetComponent<Enemy>();
        TargetEnemy.OnDamageTaken(DamageAmount);
    }

    public void SetTransform(Transform TowerTransform)
    {
        Direction = TowerTransform.forward;
        Rotation = TowerTransform.rotation;
    }
}
