using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public DynamicQuark dynamicQuark;
    public Rigidbody2D rb;
    public EnemyMovement movement;
    public bool isFollowing=true;
    public bool isHadron;
    public GameObject energyPrefab;
    public GameObject effectPrefab;
    public ParticleSystem hurteffectPrefab;
    public float staggerTime = 1f;
    public GameObject target;

    private void Start()
    {
        GameManager.instance.AddEnemyToList(this);
        dynamicQuark = GetComponent<DynamicQuark>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<EnemyMovement>();
        target = GameManager.instance.player;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ShotQuark quark = collision.gameObject.GetComponent<ShotQuark>();
        //did the enemy get hit by the players quark
        if (quark != null && quark.canDamage)
        {
            //if collision was strong enough, remove the quark
            if (StrongCollision(collision))
            {
                //if a single quark then kill the enemy
                if (dynamicQuark.qCount == 0)
                {
                    GameManager.instance.RemoveFromList(this);
                    StartCoroutine(Death());
                }
                else
                    dynamicQuark.RemoveQuark();
                //disable the collided quarks damage
                quark.canDamage = false;

                StartCoroutine(PauseMovement(staggerTime));
                //spawn a energy if its a hadron
                if(isHadron)
                    dynamicQuark.SpawnEnergy(collision);

                //play some effects
                hurteffectPrefab.Play();
                AudioManager.instance.Play("EnemyHurt");
            }
        }

        DashEnemyMovement dash = collision.gameObject.GetComponent<DashEnemyMovement>();
        //if collided with a hadron add this to the hadron
        if (dash!=null && isHadron )
        {
            //return if the hadron is full
            if (dynamicQuark.qCount+1 == dynamicQuark.qCountMax)
                return;
            //pick up this quark and destroy it
            AudioManager.instance.Play("EnemyPickUp");
            dynamicQuark.AddQuark();
            Destroy(collision.gameObject);
        }
    }

    //bool method for deciding if the collision was significant enough
    public bool StrongCollision(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude >= 1f)
            return true;
        return false;
    }

    //method for bouncing off the player when colliding
    public void KnockBack(Vector2 contact)
    {
        //play sound effects
        AudioManager.instance.Play("EnemyHurt");

        //get the vector from to the collision and apply a force in the recoil direction
        Vector3 contact3 = new Vector3(contact.x, contact.y, 0f);
        Vector3 dir = (contact3 - transform.position).normalized;

        //stop the enemy from following the player
        StartCoroutine(PauseMovement(staggerTime));
        rb.AddForce(-dir * 10f, ForceMode2D.Impulse);
    }


    //coroutine to spawn death particles then destroy the enemy object
    public IEnumerator Death()
    {
        GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Destroy(effect);
        Destroy(gameObject);
    }

    //method for stalling the enemy ai for a time then sending them in a random direction
    public IEnumerator PauseMovement(float pauseTime)
    {
        isFollowing = false;
        yield return new WaitForSeconds(pauseTime);

        isFollowing = true;
        StartCoroutine(GoRandomDirection());
    }

    //method sets target to random direction by selecting a random spawn point
    public IEnumerator GoRandomDirection()
    {
        //let the enemy head away for a time
        target = GameManager.instance.GetRandomSpawnPoint();
        yield return new WaitForSeconds(3f);
        //return to following the player
        target = GameManager.instance.player;
    }
}
