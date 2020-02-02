using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationTorso : MonoBehaviour
{
    public Animator animator;
    bool tired;
    // Start is called before the first frame update
    void Start()
    {
        tired = false;
        animator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (tired == true)
            {
                tired = false;
                animator.SetBool("Tired", false);
                //Debug.Log(tired);
            }
            else
            {
                tired = true;
                animator.SetBool("Tired", true);
                //Debug.Log(tired);
            }
        }
    }
}
