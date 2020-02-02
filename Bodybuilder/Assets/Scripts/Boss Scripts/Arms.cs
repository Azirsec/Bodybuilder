using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Arms : MonoBehaviour
{
    enum ArmStates
    {
        walking,
        slashWalking,
        slashing,
        spinning,
        tired
    }

    [SerializeField] int health;
    [SerializeField] int damage;
    [SerializeField] private float speed;

    [SerializeField] Image healthbar;

    public PlayerStats player;

    public Animator animator;

    Vector3 direction = new Vector3();

    ArmStates state = ArmStates.walking;

    float cooldownTimer = 5;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float width = (float)health / (float)200;

        healthbar.rectTransform.sizeDelta = new Vector2(width * 600, 40);

        switch (state)
        {
            case ArmStates.walking:
                direction = (player.transform.position - transform.position).normalized;
                direction.y = 0;
                gameObject.transform.position += direction * speed * Time.deltaTime;
                gameObject.transform.LookAt(transform.position + direction);

                if ((player.transform.position - transform.position).magnitude <= 15)
                {
                    RandomizeAttack();
                }

                break;

            case ArmStates.slashWalking:
                //state = ArmStates.spinning;
                direction = (player.transform.position - transform.position).normalized;
                direction.y = 0;
                gameObject.transform.position += direction * speed * 1.5f * Time.deltaTime;
                gameObject.transform.LookAt(transform.position + direction);

                if ((player.transform.position - transform.position).magnitude <= 7)
                {
                    state = ArmStates.slashing;
                    transform.Rotate(0, -15, 0);
                    cooldownTimer = 1;
                    animator.SetInteger("Attack", 2);
                }

                break;

            case ArmStates.slashing:
                //animation play
                
                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0)
                {
                    state = ArmStates.tired;
                    cooldownTimer = 3;
                    animator.SetInteger("Attack", 0);
                }

                break;

            case ArmStates.spinning:
                //run spinning animation

                direction = (player.transform.position - transform.position).normalized;
                direction.y = 0;
                gameObject.transform.position += direction * speed  * Time.deltaTime;

                gameObject.transform.Rotate(0, Time.deltaTime * 720, 0);

                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0)
                {
                    state = ArmStates.tired;
                    cooldownTimer = 3;
                    animator.SetInteger("Attack", 0);
                }

                break;

            case ArmStates.tired:
                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0)
                {
                    state = ArmStates.walking;
                }
                break;
        }
    }

 

    private void RandomizeAttack()
    {
        int thonk;
        thonk = Random.Range(0, 10);

        if (player.transform.position.y > 4)
        {
            state = ArmStates.slashWalking;
        }
        else
        {
            if (thonk < 7)
            {
                state = ArmStates.spinning;
                animator.SetInteger("Attack", 1);
                cooldownTimer = 5;
            }
            else
            {
                state = ArmStates.slashWalking;
            }
        }
    }

    public int getDamage()
    {
        return damage;
    }

    public void takeDamage(int damageIn)
    {
        health -= damageIn;
        print(health);
        if (health <= 0)
        {
            //player win
            SceneManager.LoadScene(3);
        }
    }
}
