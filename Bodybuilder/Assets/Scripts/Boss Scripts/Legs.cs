using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Legs : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int damage;
    [SerializeField] private float speed;

    [SerializeField] Image healthbar;

    [SerializeField] PlayerStats player;

    public Animator animator;

    enum LegStates
    {
        following,
        kicking,
        cooldown
    }

    LegStates state = LegStates.cooldown;

    float cooldownTimer = 5;

    bool kicking = false;

    bool playerkicked = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float width = (float)health / (float)400;

        healthbar.rectTransform.sizeDelta = new Vector2(width * 600, 40);

        switch (state)
        {
            case LegStates.following:
                Vector3 direction = (player.transform.position - transform.position).normalized;
                direction.y = 0;
                gameObject.transform.position += direction * speed * Time.deltaTime;
                gameObject.transform.LookAt(transform.position + direction);

                animator.SetBool("Walking", true);

                if ((player.transform.position - transform.position).magnitude <= 5)
                {
                    state = LegStates.kicking;
                    animator.SetBool("Walking", false);
                    animator.SetBool("Kicking", true);
                    kicking = true;
                }
                break;

            case LegStates.kicking:

                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0)
                {
                    playerkicked = false;
                    kicking = false;
                    animator.SetBool("Kicking", false);
                    state = LegStates.cooldown;
                    cooldownTimer = 5;
                }
                break;

            case LegStates.cooldown:
                cooldownTimer -= Time.deltaTime;

                if (cooldownTimer <= 0)
                {
                    state = LegStates.following;
                    cooldownTimer = 1;
                }
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (kicking && !playerkicked)
        {
            if (collision.gameObject.GetComponent<PlayerStats>() != null)
            {
                collision.gameObject.GetComponent<PlayerStats>().takeDamage(damage);
                collision.gameObject.GetComponent<PlayerMovement>().Stun(2);
                collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 10 + Vector3.up * 10, ForceMode.Impulse);
                playerkicked = true;
            }
        }
    }

    public void Kick()
    {
        player.GetComponent<PlayerStats>().takeDamage(damage);
        player.GetComponent<PlayerMovement>().Stun(2);
        player.GetComponent<Rigidbody>().AddForce(transform.forward * 10 + Vector3.up * 10, ForceMode.Impulse);
        playerkicked = true;
    }

    public bool getKicking()
    {
        return kicking;
    }

    public bool getPlayerKicked()
    {
        return playerkicked;
    }

    public int getDamage()
    {
        return damage;
    }

    public void setPlayerKicked(bool kick)
    {
        playerkicked = kick;
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
                SceneManager.LoadScene(3);
            }
        }
    }
}
