using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationLegs : MonoBehaviour
{

    public Animator animator;
    bool walking;
    bool kicking;

    float timer = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        walking = false;
        kicking = false;
        animator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(walking)
            {
                walking = false;
                Debug.Log(walking);
                animator.SetBool("Walking", false);
            }
            else if(!walking)
            {
                walking = true;
                Debug.Log(walking);
                animator.SetBool("Walking", true);
            }
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            if (kicking)
            {
                kicking = false;
                Debug.Log(kicking);
                animator.SetBool("Kicking", false);
            }
            else if (!kicking)
            {
                kicking = true;
                Debug.Log(kicking);
                animator.SetBool("Kicking", true);
            }
        }
    }
}
