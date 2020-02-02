using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour
{

    [SerializeField] Legs legs;
    [SerializeField] PlayerStats player;

    bool exists = true;
    float cooldowntimer = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!exists)
        {
            cooldowntimer -= Time.deltaTime;
            if (cooldowntimer <= 0)
            {
                exists = true;
                GetComponent<BoxCollider>().enabled = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!legs.getPlayerKicked())
        {
            if (collision.gameObject.GetComponent<PlayerStats>() != null)
            {
                legs.Kick();
                exists = false;
                cooldowntimer = 5;
                GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}
