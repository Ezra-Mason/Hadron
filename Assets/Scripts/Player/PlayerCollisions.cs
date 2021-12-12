using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    //public PlayerQuarkManager quarkManager;
    //public PlayerShoot shooting;
    public PlayerAmmo ammo;

    public GameObject pickUpEffect;
    public GameObject damageEffect;

    public bool hasDied = false;
    public bool isInvincible = false;

    public float invincibleTimer = 1f;
    public float timeInvincible = 1f;

    public void Start()
    {
        //quarkManager = GetComponent<PlayerQuarkManager>();
        //shooting = GetComponent<PlayerShoot>();
        ammo = GetComponent<PlayerAmmo>();
    }

    private void Update()
    {
        //if invincible, wiat some time then can be damaged
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if the player crosses over a quark and has space, pick it up
        if (collision.gameObject.tag == "Quark")
        {
            AudioManager.instance.Play("PickUp");
            Destroy(collision.gameObject);
            StartCoroutine(PickUp(collision));
            ammo.shots += 1;
            ammo.AddQuark();
        }
        
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        //if the collision is an enemy and the player is not invicible
        // remove a quark from the player
        if (enemy != null)
        {
            //dont remove if invicible
            if (isInvincible)
                return;
            //once hit the player becomes invincible for a bit
            isInvincible = true;
            invincibleTimer = timeInvincible;

            //if the player has no quarks and are hit, they die
            if (ammo.shots == 0 && !hasDied)
            {
                StartCoroutine(ammo.Death());
                hasDied = true;
                return;
            }

            //the player has quarks and isnt invincible, kick out a quark 
            StartCoroutine(ammo.KnockOut(collision.GetContact(0).point));
            //kock back the enmy
            enemy.KnockBack(collision.GetContact(0).point);
            //add juice: shake the screen, play a sound and spawn particles
            GameManager.instance.shaker.Shake();
            AudioManager.instance.Play("Damage");
            StartCoroutine(Damage(collision));

        }
    }

    //play a particle effect for picking up a quark
    public IEnumerator PickUp(Collision2D collision)
    {
        GameObject effect = Instantiate(pickUpEffect, collision.GetContact(0).point, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        Destroy(effect);
    }

    //play a particle effect for being damaged
    public IEnumerator Damage(Collision2D collision)
    {
        GameObject effect = Instantiate(damageEffect, collision.GetContact(0).point, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        Destroy(effect);
    }
}
