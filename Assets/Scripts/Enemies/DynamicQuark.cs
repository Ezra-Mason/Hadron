using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicQuark : MonoBehaviour
{
    //public Transform hadron;
    public GameObject[] quarks;
    public List<GameObject> quarkList = new List<GameObject>();
    public float orbitRadius = 0.25f;
    public float orbitSpeed = 1f;
    public int qCount = 2;
    public int qCountMax = 2;
    public bool isHadron;
    public GameObject energyPrefab;
    public LayerMask blockingLayer;


    // Start is called before the first frame update
    void Start()
    {

        //add the quarks to the list and order them
        for (int i = 0; i < quarks.Length; i++)
        {
            quarkList.Add(quarks[i]);
        }
        DetermineArrrangement();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //rotate the quarks around the origin of the enemy
        for (int i = 0; i < quarks.Length; i++)
        {
            quarks[i].transform.RotateAround(transform.position, Vector3.back, orbitSpeed * Time.deltaTime);
        }

    }

    //method places all the quarks in the list equally around the unit circle then scales it by a radius
    void DetermineArrrangement()
    {
        for (int i = 0; i < quarkList.Count; i++)
        {
            float x = Mathf.Cos((i / (float)quarkList.Count) * 2 * Mathf.PI);
            float y = Mathf.Sin((i / (float)quarkList.Count) * 2 * Mathf.PI);
            quarks[i].transform.localPosition = new Vector3(x, y, 0f) * orbitRadius;

        }

    }

    //add a quark to the list and reorder
    public void AddQuark()
    {
        qCount += 1;
        quarkList.Add(quarks[qCount]);
        quarks[qCount].SetActive(true);
        DetermineArrrangement();
    }


    //remove a quark from the list then reorder
    public void RemoveQuark()
    {
        quarkList.Remove(quarkList[qCount]);
        quarks[qCount].SetActive(false);
        DetermineArrrangement();
        qCount -= 1;
    }

    //method for dropping the energy
    public void SpawnEnergy(Collision2D collision)
    {
        //get the vector to the collision
        Vector2 contact = collision.GetContact(0).point;
        Vector3 contact3 = new Vector3(contact.x, contact.y, 0f);
        Vector3 dir = (contact3 - transform.position).normalized;

        //apply a random noise to the position
        Vector3 noise = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));

        //check that the spawn isnt blocked
        if (Physics2D.OverlapCircle(transform.position + (-dir * 2f + noise), 0.3f, blockingLayer))
            return;

        //instantiate the energy and add some force to it
        GameObject bullet = Instantiate(energyPrefab, transform.position + (-dir*2f + noise), Quaternion.identity);
        Rigidbody2D bulletrb = bullet.GetComponent<Rigidbody2D>();
        bulletrb.AddForce(-dir * 10f , ForceMode2D.Impulse);
    }
}
