using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAmmo : MonoBehaviour
{
    [Header("Quark Displays")]
    public GameObject[] quarks;
    public List<GameObject> quarkList = new List<GameObject>();
    public float orbitRadius = 0.25f;
    public GameObject quarkHolder;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float bulletForce = 20f;
    public int shots = 3;
    public PlayerController controller;
    public ParticleSystem shootEffect;
    public LayerMask blockingLayer;

    [Header("Dash")]
    public Transform dropPoint;
    public bool canDash = true;
    public float dashForce = 20f;
    public ParticleSystem dashEffect;

    [Header("Invincibility")]
    public bool isInvincible;
    public float iFrames;
    public float iTimer;

    [Header("Death")]
    public Animator anim;
    public float timeEmpty = 5f;
    public float duration = 5f;
    public bool isDying = false;
    public bool hasDied = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
        //add all the quarks to the player and set them to the right position
        for (int i = 0; i < quarks.Length; i++)
        {
            quarkList.Add(quarks[i]);
        }
        DetermineArrrangement();

        timeEmpty = duration;

    }

    // Update is called once per frame
    void Update()
    {
        //shoot on lft mouse and fire point is not over an object
        if (Input.GetButtonDown("Fire1") && shots > 0 && !Physics2D.OverlapCircle(shootPoint.position, 0.4f, blockingLayer))
        {
            Shoot();
        }

        //dash when the player has at least one shot and hasnt dashed too soon before
        if (Input.GetButtonDown("Jump") && shots > 0 && canDash)
        {
            StartCoroutine(Dash());
        }
        
        //if the player has no quarks, they start to die over the a time = timeEmpty
        if (timeEmpty <= 0 && shots==0 && !hasDied)
        {
            StartCoroutine(Death());
        }
        else if (shots==0)
        {
            timeEmpty -= Time.deltaTime;
        }

        //add some screenshake to show the player they are in danger of dying
        if (timeEmpty<duration)
        {
            if (!GameManager.instance.shaker.isShaking)
                GameManager.instance.shaker.StartShake();

        }
        else
        {
            if (GameManager.instance.shaker.isShaking)
                GameManager.instance.shaker.StopShake();
        }

        //timer for iframes of the player after being hurt
        if (isInvincible)
        {
            iTimer -= Time.deltaTime;
            if (iTimer < 0)
                isInvincible = false;
        }
        anim.SetInteger("Shots", shots);
        
    }

    //for the players given list of quarks sort them evenly around the unit circle then scale by a radius
    public void DetermineArrrangement()
    {
        transform.rotation = Quaternion.identity;
        for (int i = 0; i < quarkList.Count; i++)
        {
            float x = Mathf.Cos((i / (float)quarkList.Count) * 2 * Mathf.PI);
            float y = Mathf.Sin((i / (float)quarkList.Count) * 2 * Mathf.PI);
            quarkList[i].transform.localPosition = new Vector3(x, y, 0f) * orbitRadius;
        }
    }

    //method for firing the quarks from the player
    public void Shoot()
    {
        //add some shake, animation and audio queues
        AudioManager.instance.Play("Fire");
        GameManager.instance.shaker.Shake();
        shootEffect.Play();
        anim.SetTrigger("Shoot");

        //instantiate the quark and launch it with a force
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody2D bulletrb = bullet.GetComponent<Rigidbody2D>();
        bulletrb.AddForce(shootPoint.right * bulletForce, ForceMode2D.Impulse);

        //remove the quark in the players body
        RemoveQuark();
        shots -= 1;

        //check if the player is now out of quarks
        if (shots==0)
        {
            anim.SetTrigger("FinalShot");
            AudioManager.instance.Play("Dying");
        }
    }

    //Coroutine for the dash mechanic, timed so the player has no control of the player for a time
    public IEnumerator Dash()
    {
        canDash = false;
        //add some screenshake, audio effect and particles
        AudioManager.instance.Play("Fire");
        GameManager.instance.shaker.Shake();
        dashEffect.Play();
        
        //up the players drag so the dash has a sharp fall off in velocity
        controller.rb.drag = 1.5f;

        //drop a quark behind the players dash direction
        GameObject bullet = Instantiate(bulletPrefab,dropPoint.position, shootPoint.rotation);
        RemoveQuark();
        shots -= 1;

        //check if the player is now dying
        if (shots == 0)
        {
            anim.SetTrigger("FinalShot");
            AudioManager.instance.Play("Dying");
        }
        
        //stop taking player input and add the dash force
        controller.canMove = false;
        controller.rb.AddForce(shootPoint.right * bulletForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);

        //after the time, return control to the player
        controller.rb.drag = 0f;
        controller.canMove = true;
        canDash = true;


    }

    //method for placing the quark back into the players body
    public void AddQuark()
    {
        //reset dying timer
        timeEmpty = duration;
        //place a quark back in the players body and enable it
        quarkList.Add(quarks[shots-1]);
        quarks[shots-1].SetActive(true);
        //reorder the quarks to be evenly spaced with the new quark
        DetermineArrrangement();
        //stop any audio that was playing
        AudioManager.instance.Stop("Dying");
    }

    //method for removing a quark from the players body
    public void RemoveQuark()
    {
        //remove the quark from the list and set it to inactive
        quarkList.Remove(quarkList[shots-1]);
        quarks[shots-1].SetActive(false);

        //reorder the quarks in the body
        DetermineArrrangement();
    }

    //method for the player being hit by an enemy had having quarks knocked out
    public IEnumerator KnockOut(Vector2 contact)
    {
        //get the direction from the collition to the player and add a force from that direction
        Vector3 contact3 = new Vector3(contact.x, contact.y, 0f);
        Vector3 dir = (contact3 - transform.position).normalized;
        controller.rb.AddForce(-dir * bulletForce / 2, ForceMode2D.Impulse);

        //if the player isnt against a wall fire a quark in the recoil direction
        if (!Physics2D.OverlapCircle(transform.position + (-dir), 0.4f,blockingLayer))
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position + (-dir), Quaternion.identity);
            Rigidbody2D bulletrb = bullet.GetComponent<Rigidbody2D>();
            bulletrb.AddForce(-dir * bulletForce / 2, ForceMode2D.Impulse);
            RemoveQuark();
            shots -= 1;
            {
                anim.SetTrigger("FinalShot");
                AudioManager.instance.Play("Dying");
            }
        }
        yield return new WaitForSeconds(0.2f);
    }

    //method for dying sound and telling the manager the player is dead
    public void Die()
    {
        AudioManager.instance.Play("Death");
        GameManager.instance.PlayerDead();
    }

    //coroutine for playing death effects and telling the GM the player is dead
    public IEnumerator Death()
    {
        hasDied = true;
        AudioManager.instance.Play("Death");
        GameManager.instance.PlayerDead();
        yield return new WaitForSeconds(0.2f);
    }
}
