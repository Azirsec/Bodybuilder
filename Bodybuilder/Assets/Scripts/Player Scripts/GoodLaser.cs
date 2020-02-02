using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodLaser : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private float lifetime;
    [SerializeField] private ParticleSystem ps;

    float timer = 1;
    bool timerStart = false;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;

        if (lifetime <= 0 && !timerStart)
        {
            Destroy(gameObject);
        }
        if (timerStart)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    public int getDamage()
    {
        return damage;
    }

    public void Explode()
    {
        //make explodey happen
        ps.Play();
        timerStart = true;

        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<MeshCollider>().enabled = false;

        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        //gameObject.transform.forward = Vector3.up;
    }
}
