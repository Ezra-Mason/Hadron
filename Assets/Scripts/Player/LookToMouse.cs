using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookToMouse : MonoBehaviour
{
    public Camera cam;
    Vector2 mousePos;
    public Vector2 lookDir;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }
    void FixedUpdate()
    {
        lookDir = (mousePos - (Vector2)transform.position).normalized;
        anim.SetFloat("Look X", lookDir.x);
        anim.SetFloat("Look Y", lookDir.y);
    }
}
