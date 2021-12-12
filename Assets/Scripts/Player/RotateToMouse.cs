using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    public Camera cam;
    Vector2 mousePos;
    public float rotateSpeed = 10f;
    public Transform arm;
    // Start is called before the first frame update
    void Start()
    {
        arm = GetComponentInChildren<Transform>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        //direction to mouse position
        Vector2 lookDir = mousePos - (Vector2)transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //smoothly rotate the mouse to that position
        arm.rotation = Quaternion.Slerp(arm.rotation, rotation, rotateSpeed * Time.deltaTime);
    }
}
