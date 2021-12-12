using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public Rigidbody2D rb;
    public float speed = 2f;
    public DynamicQuark dynamicQuark;
    public Enemy enemy;
    //public bool isFollowing = true;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player.transform;
        dynamicQuark = GetComponent<DynamicQuark>();
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        //if the enemy is meant to be following move towards the players current position
        if (enemy.isFollowing)
        {
            Vector3 move = enemy.target.transform.position - transform.position;
            rb.MovePosition(transform.position + move * speed * Time.deltaTime);
        }
    }

}
