using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadWeakspot : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private Head head;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player)
        {
            head.takeDamage();
        }
    }
}
