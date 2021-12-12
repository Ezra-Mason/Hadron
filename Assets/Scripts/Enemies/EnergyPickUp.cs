using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPickUp : MonoBehaviour
{
    public float waitTime = 5f;
    public float timer;
    public float currentTime;
    public GameObject enemyPrefab;
    public GameObject effectPrefab;
    public bool hasSpawned = false;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = waitTime;        
    }

    // Update is called once per frame
    void Update()
    {
        //wait for a certain amount of time, then spawn the quark
        if (currentTime<=0 && !hasSpawned)
        {
            StartCoroutine(SpawnEnemy());
        }
        else
        {
            currentTime -= Time.deltaTime;
        }
    }

    //coroutine for spawning effects and new enemy, then destroying this object
    public IEnumerator SpawnEnemy()
    {
        hasSpawned = true;
        GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Destroy(effect);

        Instantiate(enemyPrefab,transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController controller = collision.gameObject.GetComponent<PlayerController>();
        //if the player collides with this then this dies
        if (controller != null)
        {
            StartCoroutine(Die());
        }

    }

    //soroutine spawns effects then destroys this object
    public IEnumerator Die()
    {
        //death particles
        AudioManager.instance.Play("EnemyHurt");
        GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(0.2f);

        Destroy(effect);
        Destroy(gameObject);
    }
}
