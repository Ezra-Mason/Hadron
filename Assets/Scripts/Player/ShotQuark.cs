using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotQuark : MonoBehaviour
{

    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public bool canDamage;
    public Sprite inactiveSprite;
    public GameObject effect;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //disable damage if the quark is no longer moving
        if (canDamage && rb.velocity.magnitude==0.1f)
        {
            canDamage = false;
        }

        //change sprite for inactive quarks
        if (!canDamage)
        {
            sprite.sprite = inactiveSprite;
            effect.SetActive(false);
        }
    }

    //play a sound if the quark hits something while active
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canDamage)
            AudioManager.instance.Play("Bounce");
    }
}
