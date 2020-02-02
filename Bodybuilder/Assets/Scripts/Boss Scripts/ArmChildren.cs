using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmChildren : MonoBehaviour
{
    [SerializeField] Arms parent;

    float cooldown = 0;
    bool canHit = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;
        if (cooldown <= 0)
        {
            canHit = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerStats>() != null)
        {
            if (canHit)
            {
                collision.gameObject.GetComponent<PlayerStats>().takeDamage(parent.getDamage());
                canHit = false;
                cooldown = 0.5f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GoodLaser>() != null)
        {
            parent.takeDamage(other.GetComponent<GoodLaser>().getDamage());
            other.GetComponent<GoodLaser>().Explode();
        }
    }
}
