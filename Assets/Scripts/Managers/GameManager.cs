using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public GameObject deathScreen;
    public List<Enemy> enemies = new List<Enemy>();
    public int playerScore = 0;
    public GameObject player;
    public Camera cam;
    public CameraShake shaker;
    public GameObject[] spawnPoints;

    private void Awake()
    {
        //singleon
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        //clear enemies
        enemies.Clear();
    }

    //show the "you died" screen when the player dies
    public void PlayerDead()
    {
        GameObject.Find("Canvas").GetComponent<GameMenuManager>().ShowDeathScreen();
    }


    //method called by an enemy when in its Start to add it to the enemies list
    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    //method called by an enemy when it dies to remove it from the enemies list
    public void RemoveFromList(Enemy script)
    {
        enemies.Add(script);
    }

    // returns a random spawn position from all the positions
    public GameObject GetRandomSpawnPoint()
    {
        int rand = Random.Range(0, spawnPoints.Length);
        return spawnPoints[rand];
    }

}
