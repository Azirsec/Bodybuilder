using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Torso : MonoBehaviour
{
    public GameObject player;
    public GameObject torso;
    public GameObject offset;
    public GameObject ft;

    //Rolling attack variables
    Vector3 rollingTarget;
    public float cooldown;
    float fallingSpeed;
    float rollingDelay;
    float rollingSpeed;
    float minRollingTime;
    bool fallingOver;
    public bool rolling;
    bool gettingUp;

    //Ground pound attack variables
    Vector3 slamHeight;
    float ascendTimer;
    float descendTimer;
    bool goingUp;

    //Womp attack variables
    float vibrateTimer;
    float pauseTimer;
    float fallingTimer;
    float risingTimer;
    bool vibrating;
    bool waiting;
    bool falling;
    bool pancake;
    bool rising;

    //1 for roll
    //2 for groundPound
    //3 for womp;
    int moves;

    bool attacking;

    Vector2 lookingDirection;
    float percentage;
    Vector3 initialPosition;

    //CHRIS THIS BOOL WILL LET THE BOSS KNOW THAT CONTACT WITH THE PLAYER WILL DEAL DAMAGE
    public bool canDamagePlayer;

    [SerializeField] Image healthbar;

    [SerializeField] int health;
    [SerializeField] int slamDamage;
    public int rollDamage;

    float tiredTimer;
    bool tired;

    private void Start()
    {
        cooldown = 5.0f;

        fallingSpeed = 2.0f;
        rollingSpeed = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        float width = (float)health / (float)1000;

        healthbar.rectTransform.sizeDelta = new Vector2(width * 600, 40);

        if (tired)
        {
            //CHRIS MAKE IT DO THE TIRED BREATHING ANIMATION IN HERE

            tiredTimer -= Time.deltaTime;

            if (tiredTimer <= 0.0f)
            {
                tired = false;
            }
        }

        else
        {
            cooldown -= Time.deltaTime;

            if (!rolling && !waiting && !falling && !rising && !pancake)
            {
                lookingDirection.x = player.transform.position.x - gameObject.transform.position.x;
                lookingDirection.y = player.transform.position.z - gameObject.transform.position.z;
                lookingDirection = lookingDirection.normalized;
                gameObject.transform.forward = new Vector3(lookingDirection.x, 0, lookingDirection.y);
            }

            if (!attacking && cooldown <= 0.0f && !tired)
            {

                moves = pickMove();
                attacking = true;
                cooldown = Random.Range(2, 4);

                Vector2 distanceToPlayer = new Vector2(player.transform.position.x - gameObject.transform.position.x, player.transform.position.z - gameObject.transform.position.z);
                float distance = distanceToPlayer.magnitude;

                //CHRIS THIS IS HOW HE PICKS MOVES
                //Right now it's a 60% chance to do the womp attack if the player is 10 units away
                //if you change this make sure you change the switch statements too because they
                //      depend on this number
                if (distance <= 10.0f)
                {
                    moves = Random.Range(1, 6);
                }

                else
                {
                    moves = Random.Range(1, 3);
                }

                setVariables(moves);
            }

            if (attacking && cooldown < 0.0f)
            {
                attack(moves);
            }
        }
    }

    int pickMove()
    {
        return 0;
    }

    void setVariables(int attack)
    {
        switch (attack)
        {
            case 1:
                fallingOver = true;
                rolling = false;
                gettingUp = false;
                initialPosition = offset.transform.position;
                rollingTarget = ft.transform.position;
                percentage = 0;
                rollingDelay = 1.0f;
                minRollingTime = 0.3f;
                break;
            case 2:
                percentage = 0;
                slamHeight = torso.transform.position + new Vector3(0, 8, 0);
                initialPosition = torso.transform.position;
                ascendTimer = 3.0f;
                descendTimer = 0.3f;
                goingUp = true;
                break;
            case 3:
            case 4:
            case 5:
                percentage = 0.0f;
                vibrateTimer = 1.0f;

                vibrateTimer = Mathf.PI / 2;

                pauseTimer = 1.0f;        
                fallingTimer = 0.2f;
                risingTimer = 2.0f;
                vibrating = true;
                waiting = false;
                falling = false;
                rising = false;
                initialPosition = gameObject.transform.position;

                print(gameObject.transform.right);
                break;
            default:
                break;
        }

    }

    void attack(int attack)
    {
        switch (attack)
        {
            case 1:
                if (fallingOver)
                {
                    percentage += Time.deltaTime * fallingSpeed;

                    if (percentage > 1.0f)
                    {
                        percentage = 1;
                        fallingOver = false;
                    }

                    float angle = -90 * percentage;
                    Vector3 currentPosition = (percentage * rollingTarget) + ((1 - percentage) * initialPosition);
                    torso.transform.localRotation = Quaternion.Euler(0, 0, angle);
                    offset.transform.position = currentPosition;
                }

                if (!fallingOver)
                {
                    rollingDelay -= Time.deltaTime;
                }

                if (rollingDelay < 0.0f && gettingUp == false && !rolling)
                {
                    rolling = true;
                    canDamagePlayer = true;
                }

                if (rolling)
                {
                    minRollingTime -= Time.deltaTime;
                    gameObject.transform.position += gameObject.transform.forward * rollingSpeed * Time.deltaTime;
                    torso.transform.Rotate(0, 360 * Time.deltaTime, 0);
                }

                if (gettingUp)
                {
                    torso.transform.localRotation = Quaternion.Euler(0, 0, -90);
                    if (rollingDelay < 0.0f)
                    {
                        percentage += Time.deltaTime;

                        if (percentage > 1.0f)
                        {
                            percentage = 1.0f;
                            attacking = false;
                            tired = true;
                            tiredTimer = 5.0F;
                        }

                        float angle = (1 - percentage) * -90;
                        Vector3 currentPosition = (percentage * rollingTarget) + ((1 - percentage) * initialPosition);
                        torso.transform.localRotation = Quaternion.Euler(0, 0, angle);
                        offset.transform.localPosition = currentPosition;
                    }
                }
                break;
            case 2:
                if (goingUp)
                {
                    percentage += Time.deltaTime / ascendTimer;

                    if (percentage > 1.0f)
                    {
                        percentage = 1.0f;
                        goingUp = false;
                        canDamagePlayer = true;
                    }

                    torso.transform.position = percentage * slamHeight + ((1 - percentage) * initialPosition);
                }
                else
                {
                    percentage -= Time.deltaTime / descendTimer;

                    if (percentage < 0.0f)
                    {
                        percentage = 0.0f;
                        attacking = false;
                        canDamagePlayer = false;
                        tired = false;
                        tiredTimer = 5.0f;

                        //CHRIS MAKE THE PLAYER TAKES DAMAGE IF THEY'RE ON THE GROUND WHEN THIS HAPPENS
                        player.GetComponent<PlayerStats>().groundSlammed(slamDamage);
                    }

                    torso.transform.position = percentage * slamHeight + ((1 - percentage) * initialPosition);
                }
                break;

            case 3:
            case 4:
            case 5:
                if (vibrating)
                {
                    vibrateTimer -= Time.deltaTime;
                    torso.transform.position = initialPosition + (gameObject.transform.right * 0.5f * Mathf.Sin(vibrateTimer * 20));
                    
                    if (vibrateTimer < 0.0f)
                    {
                        vibrateTimer = 0.0f;
                        torso.transform.localPosition = new Vector3(0, 0, 0);
                        vibrating = false;
                        waiting = true;
                    }
                }

                else if (waiting)
                {
                    pauseTimer -= Time.deltaTime;

                    if (pauseTimer < 0.0f)
                    {
                        waiting = false;
                        falling = true;
                    }
                }

                else if (falling)
                {
                    percentage += Time.deltaTime / fallingTimer;

                    if (percentage > 1.0f)
                    {
                        percentage = 1.0f;
                        falling = false;
                        pancake = true;
                        pauseTimer = 1.0f;
                    }

                    float angle = 90 * percentage;
                    torso.transform.localRotation = Quaternion.Euler(angle, 0, 0);
                }

                else if (pancake)
                {
                    pauseTimer -= Time.deltaTime;

                    if (pauseTimer < 0.0f)
                    {
                        pancake = false;
                        rising = true;
                    }
                }

                else if (rising)
                {
                    percentage -= Time.deltaTime / risingTimer;

                    if (percentage < 0.0f)
                    {
                        percentage = 0.0f;
                        rising = false;
                        attacking = false;
                        //NOT TIRED ON PURPOSE :] 
                        //you could easily abuse this move if he was :]
                    }

                    float angle = 90 * percentage;
                    torso.transform.localRotation = Quaternion.Euler(angle, 0, 0);
                }
                break;
         default:
                break;
        }

    }

    public void stopRolling()
    {
        if (minRollingTime < 0.0f)
        {
            rolling = false;
            gettingUp = true;
            rollingDelay = 2.0f;
            percentage = 0.0f;
            initialPosition = offset.transform.localPosition;
            rollingTarget = new Vector3(0, 0, 0);
            canDamagePlayer = false;
        }
    }

  

    public void takeDamage(int damageIn)
    {
        health -= damageIn;
        if (health <= 0)
        {
            //player win
            Destroy(gameObject);
        }
    }
}
