using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemyMovement : MonoBehaviour
{
    public Transform player;
    public Vector3 target;
    public Vector3 move;
    public Rigidbody2D rb;
    public float maxSpeed = 2f;
    public float speed = 2f;
    public DynamicQuark dynamicQuark;
    public Enemy enemy;
    public float dashTime = 2f;
    public float timer=2f;
    public float waitTime =2f;
    public float waitTimer=2f;
    public bool canMove;
    public enum State { Waiting, Moving}
    public State moveState;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player.transform;
        dynamicQuark = GetComponent<DynamicQuark>();
        enemy = GetComponent<Enemy>();
        target = GameManager.instance.GetRandomSpawnPoint().GetComponent<Transform>().position;
        move = (target - transform.position).normalized;
        timer = dashTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveState == State.Moving)
        {
            if (timer <= 0 )
            {
                //move back to the waiting state when in the moving state for long enough
                moveState = State.Waiting;
                timer = dashTime;
            }
            else
            {
                //reduce the timer and then increase the speed to max to make the enemy dash
                timer -= Time.deltaTime;
                speed = Mathf.Lerp(speed, maxSpeed, 0.1f);
            }

        }
        else
        {
            //if the enemy has waited for long enough go to the moving state
            if (waitTimer <= 0)
            {
                moveState = State.Moving;
                waitTimer = waitTime;
            }
            else
            {
                //reduce the timer and set the speed to zero
                waitTimer -= Time.deltaTime;
                speed = Mathf.Lerp(speed, 0f,0.1f);

                //get the move states target position
                target = player.position;
                move = (target - transform.position).normalized;

            }
        }


        //if the enemy is not following set the movement to waiting
        if (!enemy.isFollowing)
        {
            moveState = State.Waiting;
        }

    }

    private void FixedUpdate()
    {
        //when in the moving state, move to the position the player
        if (moveState==State.Moving)
            rb.MovePosition(transform.position + move * speed * Time.deltaTime);

    }

    //coroutine for setting the enemies speed to zero for some time
    public IEnumerator PauseMovement()
    {
        float tempSpeed = speed;
        speed = 0f;
        yield return new WaitForSeconds(0.5f);

        speed = tempSpeed;
        target = player.position;
        timer = dashTime;
        canMove = true;
    }

    //when collided with something, go to the waiting state
    private void OnCollisionEnter2D(Collision2D collision)
    {
        moveState = State.Waiting;
    }

}
