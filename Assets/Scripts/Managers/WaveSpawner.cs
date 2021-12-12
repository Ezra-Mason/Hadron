using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    //wave class for customising the waves in the inspector
    [System.Serializable]
    public class Wave
    {
        public string name;
        public GameObject[] enemies;
        public int[] amount;
        public float rate;

    }
    public Wave[] waves;
    public int nextWave = 0;
    public float timeBetweenWaves = 5f;
    private float waveCountDown = 0f;

    public GameObject waveTextObject;
    public Animator anim;
    public Text waveText;

    public enum SpawnState { Spawning, Waiting,Counting}
    public SpawnState state = SpawnState.Counting;

    public Transform[] spawnPoints;
    public List<Transform> spawnPointList = new List<Transform>();

    private float searchCountdown = 1f;

    public GameMenuManager menuManager;

    // Start is called before the first frame update
    void Start()
    {
        waveCountDown = timeBetweenWaves;
        PopulateSpawnPoints();

        if (spawnPoints.Length == 0)
            Debug.LogError("No spawn points referenced");

        anim = waveTextObject.GetComponent<Animator>();
        waveText = waveTextObject.GetComponent<Text>();
        anim.SetBool("IsHidden", true);

    }

    // Update is called once per frame
    void Update()
    {
        //while waiting check if all the enemies are dead
        if (state==SpawnState.Waiting)
        {
            if (!EnemyIsAlive())
            {
                //no enemies alive, start the new wave
                WaveCompleted();
            }
            else
                return;
            
        }

        if (waveCountDown<=0) // if timer has run out
        {
            //have we already started spawning?
            if (state!=SpawnState.Spawning)
            {
                //start spawning wave
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        { // continue to reduce the countdown
            waveCountDown -= Time.deltaTime;
        }
    }

    //method searching for enemies in the scene at a given inteval
    public bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown<=0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy")==null)
            {
                return false;
            }
        }
        return true;
    }

    //method for determining if finished or should be starting a count down
    void WaveCompleted()
    {
        Debug.Log("WaveCompleted");

        state = SpawnState.Counting;
        waveCountDown = timeBetweenWaves;

        //if that was the last wave call the finished method or move on to the next wave
        if (nextWave+1>waves.Length-1)
        {
            FinishedWaves();
        }
        else
        {
            nextWave++;
            PopulateSpawnPoints();
        }
        anim.SetBool("IsHidden", true);
    }

    //coroutine for spawning wave memebers
    public IEnumerator SpawnWave(Wave _wave)
    {
        //display the waves name
        Debug.Log("Spawning Wave " + _wave.name);
        waveText.text = _wave.name;
        anim.SetBool("IsHidden", false);

        state = SpawnState.Spawning;
        
        //spawn all the enemies in the wave for the given amounts
        for (int i = 0; i < _wave.enemies.Length; i++)
        {
            for (int j = 0; j < _wave.amount[i]; j++)
            {
                SpawnEnemy(_wave.enemies[i]);
                yield return new WaitForSeconds(1 / _wave.rate);
            }
        }

        //now wait for the player to fight them 
        state = SpawnState.Waiting;
        yield break;
    }


    //instantiate the given enemy at a random spawn point
    void SpawnEnemy(GameObject _enemy)
    {

        Transform rp = spawnPointList[Random.Range(0, spawnPointList.Count)];
        Instantiate(_enemy, rp.position, Quaternion.identity);
        //remove the points from possible spawn locations
        spawnPointList.Remove(rp);

    }

    //method for showing the win screen
    void FinishedWaves()
    {
        menuManager.ShowWinScreen();
        Debug.Log("All waves completed, now looping");
    }

    //method for adding all the spawn points to the possible locations locations to spawn
    void PopulateSpawnPoints()
    {
        spawnPointList.Clear();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPointList.Add(spawnPoints[i]);
        }
    }
}
