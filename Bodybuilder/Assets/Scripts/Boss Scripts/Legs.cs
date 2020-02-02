using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legs : MonoBehaviour
{

    [SerializeField] private int health;
    [SerializeField] private int damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GoodLaser>() != null)
        {
            health -= other.GetComponent<GoodLaser>().getDamage();
            other.GetComponent<GoodLaser>().Explode();

            if (health <= 0)
            {
                //player win
                print("The Player Wins");
            }
        }
    }
}
