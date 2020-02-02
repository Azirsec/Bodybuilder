using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Head : MonoBehaviour
{
    enum HeadState
    {
        NotTired,
        Tired,
        PreparingToFire,
        Firing
    }

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject lefteye;
    [SerializeField] private GameObject righteye;

    [SerializeField] Image healthbar;

    [SerializeField] Transform knockbackpos;

    [SerializeField] private HeadLaser laser;

    [SerializeField] private Material safeLaser;
    [SerializeField] private Material dangerousLaser;

    private Vector3 aimlocation;

    private int health = 3;

    [SerializeField] private LineRenderer line;
    [SerializeField] private LineRenderer line2;

    Vector3 righteyetoaim;
    Vector3 lefteyetoaim;

    public Animator animator;

    [SerializeField] Transform weakspot;

    HeadState state = HeadState.NotTired;

    int maxShots = 3;
    int shotsLeft = 3;

    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        line.startColor = dangerousLaser.color;
        line.endColor = dangerousLaser.color;
        line2.startColor = dangerousLaser.color;
        line2.endColor = dangerousLaser.color;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        float width = (float)health / (float)3;
        print(width);

        healthbar.rectTransform.sizeDelta = new Vector2(width * 600, 40);

        switch (state)
        {
            case HeadState.NotTired:

                //create a line from the eyes to the player

                aimlocation = player.transform.position - new Vector3(0, 0.5f, 0);
                transform.LookAt(player.transform.position - new Vector3(0, 0.5f, 0));

                if (timer > 3.0f)
                {
                    state = HeadState.PreparingToFire;
                    timer = 0;
                }
                break;
            case HeadState.PreparingToFire:

                aimlocation = player.transform.position - new Vector3(0, 0.5f, 0);
                transform.LookAt(aimlocation);

                //make laser lines visible
                line.enabled = true;
                line2.enabled = true;

                line.SetPosition(0, righteye.transform.position);
                line.SetPosition(1, aimlocation);

                line2.SetPosition(0, lefteye.transform.position);
                line2.SetPosition(1, aimlocation);

                if (timer > 1.0f)
                {
                    state = HeadState.Firing;
                    timer = 0;

                    //make lines wider and more red? Old laser tracking stuff
                    /*
                    line.startWidth = 0.15f;
                    line.endWidth = 0.15f;
                    line2.startWidth = 0.15f;
                    line2.endWidth = 0.15f;
                    line.startColor = dangerousLaser.color;
                    line.endColor = dangerousLaser.color;
                    line2.startColor = dangerousLaser.color;
                    line2.endColor = dangerousLaser.color;
                    */
                }
                break;
            case HeadState.Firing:
                // fire off the raycasts and damage player if hit
                //aimlocation += (player.transform.position - aimlocation).normalized * 5.0f * Time.deltaTime;

                aimlocation = player.transform.position - new Vector3(0, 0.5f, 0);
                transform.LookAt(aimlocation);
                
                line.enabled = false;
                line2.enabled = false;

                Quaternion direction = new Quaternion();
                direction.SetLookRotation(aimlocation - righteye.transform.position);
                Instantiate(laser, righteye.transform.position + transform.forward, direction);

                direction.SetLookRotation(aimlocation - lefteye.transform.position);
                Instantiate(laser, lefteye.transform.position + transform.forward, direction);

                print(shotsLeft);
                shotsLeft -= 1;
                if (shotsLeft <= 0)
                {
                    shotsLeft = maxShots;
                    state = HeadState.Tired;
                    animator.SetBool("Tired", true);

                    transform.forward = new Vector3(0, 0, 1);
                    weakspot.transform.position += transform.forward;
                }
                else
                {
                    state = HeadState.NotTired;
                }

                /*
                line.SetPosition(0, righteye.transform.position);
                line.SetPosition(1, aimlocation);

                line2.SetPosition(0, lefteye.transform.position);
                line2.SetPosition(1, aimlocation);*/

                //if (timer > 1.0f)
                //{
                timer = 0;

                    //turn off eye lasers once they fire and reset their width. old laser stuff
                    /*line.enabled = false;
                    line2.enabled = false;
                    line.startWidth = 0.1f;
                    line.endWidth = 0.1f;
                    line2.startWidth = 0.1f;
                    line2.endWidth = 0.1f;

                    line.startColor = safeLaser.color;
                    line.endColor = safeLaser.color;
                    line2.startColor = safeLaser.color;
                    line2.endColor = safeLaser.color;*/
                //}
                break;
            case HeadState.Tired:
                //boss plays lean forward animation and waits for a few seconds
                if (timer >= 8)
                {
                    state = HeadState.NotTired;
                    animator.SetBool("Tired", false);
                    weakspot.transform.position -= transform.forward;
                }

                break;
        }

    }

    public void takeDamage()
    {
        health -= 1;

        player.GetComponent<PlayerMovement>().Stun(3);
        player.transform.position = knockbackpos.position;
        player.GetComponent<Rigidbody>().velocity = new Vector3();
        player.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, 2) * 5, ForceMode.Impulse);

        if (health <= 0)
        {
            SceneManager.LoadScene(1);
        }
    }
}
