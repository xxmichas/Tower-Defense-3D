using UnityEngine;

public class Tower4 : MonoBehaviour
{
    public float AttackCooldown = 2f;
    public GameObject ArrowPrefab;
    public Transform ArrowSpawnPoint;

    public AudioSource AttackSound;

    private void OnEnable()
    {
        InvokeRepeating("Attack", 0f, AttackCooldown);
    }

    void Attack()
    {
        GameObject BulletObject = (GameObject)Instantiate(ArrowPrefab, ArrowSpawnPoint.position, ArrowPrefab.transform.rotation);
        BulletObject.GetComponent<Arrow>().SetTransform(transform);

        AttackSound.Play();
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
