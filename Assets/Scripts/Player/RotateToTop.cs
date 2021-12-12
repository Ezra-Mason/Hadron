using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToTop : MonoBehaviour
{
    public GameObject holder;


    // Update is called once per frame
    void Update()
    {
        //counter the rotation of the holder so the quark sprites stay upright
        if (holder.transform.rotation.z!=0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -holder.transform.rotation.z);
        }
    }
}
