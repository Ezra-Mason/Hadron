using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookToMovement : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        //get the direction to the player and set the animation direction to it
        Vector2 dir = (player.transform.position - transform.position).normalized;
        anim.SetFloat("Look X", dir.x);
        anim.SetFloat("Look Y", dir.y);
    }
}
