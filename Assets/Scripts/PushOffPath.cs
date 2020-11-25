using UnityEngine;

public class PushOffPath : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 RandomImpule;

    private void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        RandomImpule = new Vector3(Random.Range(-0.1f, 0.1f), 0.3f, Random.Range(-0.1f, 0.1f));
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.layer == 11)
        {
            rb.AddForce(RandomImpule, ForceMode.Impulse);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        RandomImpule = new Vector3(Random.Range(-0.1f, 0.1f), 0.3f, Random.Range(-0.1f, 0.1f));
    }
}
