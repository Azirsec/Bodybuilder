using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorsoChild : MonoBehaviour
{
    public Torso parent;

    private void OnTriggerExit(Collider other)
    {
        if (parent.rolling && other.gameObject.tag == "Finish")
        {
            parent.stopRolling();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == parent.player.gameObject)
        {
            if (parent.canDamagePlayer)
            {
                parent.player.GetComponent<PlayerStats>().takeDamage(parent.rollDamage);
                parent.canDamagePlayer = false;
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
