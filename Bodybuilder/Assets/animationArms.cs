using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationArms : MonoBehaviour
{
    public Animator animator;
    int attackState;
    // Start is called before the first frame update
    void Start()
    {
        attackState = 0;
        animator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetInteger("Attack", 0);
            
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetInteger("Attack", 1);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            animator.SetInteger("Attack", 2);
        }
    }
}
