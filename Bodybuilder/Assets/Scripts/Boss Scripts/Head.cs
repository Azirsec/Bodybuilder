using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                transform.forward = new Vector3(0, 0, 1);
                if (timer >= 8)
                {
                    state = HeadState.NotTired;
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
