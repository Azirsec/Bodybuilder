using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorsoChild : MonoBehaviour
{
    public Torso parent;

    private void OnTriggerExit(Collider other)
    {
        if (parent.rolling)
        {
            parent.stopRolling();
        }
    }
}
