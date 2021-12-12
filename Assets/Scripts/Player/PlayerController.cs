using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float horizontal;
    public float vertical;
    public float speed = 5f;
    public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //get movement input
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        //store movement in one variable
        Vector2 move = new Vector2(horizontal, vertical);
    }

    void FixedUpdate()
    {
        Vector2 position = rb.position;
        //new position after movement
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        //if player can move, apply that movement
        if (canMove == true)
            rb.position = position;
        //if the player has been pushed by something remove the induced velocity
        if (canMove==true && rb.velocity.magnitude>0  )
        {
            rb.velocity = Vector2.zero;
        }
    }

}
