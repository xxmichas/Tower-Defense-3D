using UnityEngine;

public class Tower5 : MonoBehaviour
{
    public float MaxRange = 1.5f;
    //Dealt 50 times per second
    public float DamageAmount = 2f;

    private int EnemyLayer = 1 << 12;

    public Transform BoxCenterSpawnPoint;

    public GameObject Particle1;
    public GameObject Particle2;
    public GameObject Particle3;

    private void FixedUpdate()
    {
        Damage();
    }

    private void Damage()
    {
        Collider[] Enemies = Physics.OverlapBox(BoxCenterSpawnPoint.position, new Vector3(0.95f, 0.5f, 0.95f), BoxCenterSpawnPoint.rotation, EnemyLayer);

        foreach (Collider Enemy in Enemies)
        {
        if (Vector3.Distance(transform.position, Enemy.ClosestPointOnBounds(transform.position)) <= MaxRange)
            {
                Enemy.GetComponent<Enemy>().OnDamageTaken(DamageAmount);
            }
        }
    }

    private void OnDisable()
    {
        Particle1.SetActive(false);
        Particle2.SetActive(false);
        Particle3.SetActive(false);
    }

    private void OnEnable()
    {
        Particle1.SetActive(true);
        Particle2.SetActive(true);
        Particle3.SetActive(true);
    }
}