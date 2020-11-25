using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#pragma warning disable 0168
public class Enemy : MonoBehaviour
{
    [Header("Enemy Attributes")]

    //Te wartości modyfikujemy na prefabie poszczególnych typów przeciwników np. (zwykły, szybki, wolny itp.).
    public float basespeed = 0f;
    public float basehealth = 0;
    public int bounty = 0;
    public bool SpawnBabies = false;
    public int BabiesAmount = 0;
    public float SpawnInterval = 0f;
    public GameObject BabyPrefab;
    private GameObject TempBaby;
 
    private float speed;
    private float health;

    [Header("Dont Change")]
    public Transform EnemyCanvas;
    public Image HealthBar;
    private Camera PlayerCamera;
    private Transform target;
    private int WaypointIndex = 0;

    private bool Dead;

    public GameObject DeathParticles;

    void Start()
    {
        speed = basespeed;
        health = basehealth;

        Dead = false;

        PlayerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        if(target == null)
        {
            target = Waypoints.WaypointsArray[0];
        }

        if(!Dead)
        {
            Vector3 Direction = target.position - transform.position;
            transform.Translate(Direction.normalized * speed * Time.deltaTime, Space.World);

            if (Vector3.Distance(transform.position, target.position) <= 0.05f)
            {
                NextWaypoint();
            }

            EnemyCanvas.LookAt(EnemyCanvas.position + PlayerCamera.transform.rotation * Vector3.back, PlayerCamera.transform.rotation * Vector3.up);
        }
        else
        {
            tag = "Untargetable";
            gameObject.layer = 13;
            EnemyCanvas.gameObject.SetActive(false);

            transform.Translate(transform.up * (speed / 5) * Time.deltaTime, Space.World);
        }
    }

    void NextWaypoint()
    {
        if (WaypointIndex >= Waypoints.WaypointsArray.Length - 1)
        {
            EndReached();
            return;
        }

        WaypointIndex++;
        target = Waypoints.WaypointsArray[WaypointIndex];
    }

    void EndReached()
    {
        Stats.Lives -= (1 + BabiesAmount);
        WaveSpawner.TotalEnemies -= (1 + BabiesAmount);

        try
        {
            GameManager.DamageSound();
        }
        catch (System.Exception e)
        {

        }
        Destroy(gameObject);
    }

    public void OnDamageTaken(float amount)
    {
        health -= amount;

        HealthBar.fillAmount = health / basehealth;

        if (health <= 0)
        {
            Die();
        }
    }

    public void OnSlow(float amount)
    {
        speed *= amount;
    }

    public void UnSlow()
    {
        speed = basespeed;
    }

    void Die()
    {
        if (!Dead)
        {
            Dead = true;
            Stats.Money += (bounty + Stats.MoneyBonus);
            if (SceneManager.sceneCountInBuildSettings == (SceneManager.GetActiveScene().buildIndex + 1))
            {
                TutorialManager.TotalEnemies--;
            }
            else
            {
                WaveSpawner.TotalEnemies--;
            }

            Instantiate(DeathParticles, transform.position, transform.rotation);

            if (SpawnBabies)
            {
                StartCoroutine(CreateBabies());
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator CreateBabies()
    {
        for (int j = 0; j < BabiesAmount; j++)
        {
            yield return new WaitForSeconds(SpawnInterval);
            TempBaby = (GameObject)Instantiate(BabyPrefab, transform.position, transform.rotation);
            TempBaby.GetComponent<Enemy>().target = target;
            TempBaby.GetComponent<Enemy>().WaypointIndex = WaypointIndex;
        }
        Destroy(gameObject);
    }
}
