using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject horizontalRotation;
    Rigidbody rb;
    [SerializeField] private float speed;

    bool unlocked = false;

    [SerializeField] bool haslegs = false;

    ///dash things
    [SerializeField] float dashspeed;
    [SerializeField] Material dashcolour;
    [SerializeField] Material regularcolour;
    bool dashing = false;
    float dashduration = 0.15f;
    float dashtimer = 0;
    float dashcooldown = 5;
    bool candash = true;
    Vector2 direction;
    Vector3 dashDirection;

    bool grounded = true;

    bool stunned = false;
    float stunduration = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        unlocked = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (unlocked)
        {
            if (!stunned)
            {
                if (!dashing)
                {
                    Vector3 horMov = new Vector3(gameObject.GetComponent<Rigidbody>().velocity.x, 0, gameObject.GetComponent<Rigidbody>().velocity.z);

                    if (Input.GetKey(KeyCode.W) && horMov.magnitude < speed)
                    {
                        gameObject.GetComponent<Rigidbody>().AddForce(horizontalRotation.transform.forward * speed / 10.0f, ForceMode.Impulse);
                        //transform.localPosition += horizontalRotation.transform.forward * speed * Time.deltaTime;
                    }

                    if (Input.GetKey(KeyCode.A) && horMov.magnitude < speed)
                    {
                        gameObject.GetComponent<Rigidbody>().AddForce(-horizontalRotation.transform.right * speed / 10.0f, ForceMode.Impulse);
                        //transform.localPosition += -horizontalRotation.transform.right * speed * Time.deltaTime;
                    }

                    if (Input.GetKey(KeyCode.S) && horMov.magnitude < speed)
                    {
                        gameObject.GetComponent<Rigidbody>().AddForce(-horizontalRotation.transform.forward * speed / 10.0f, ForceMode.Impulse);
                        //transform.localPosition += -horizontalRotation.transform.forward * speed * Time.deltaTime;
                    }

                    if (Input.GetKey(KeyCode.D) && horMov.magnitude < speed)
                    {
                        gameObject.GetComponent<Rigidbody>().AddForce(horizontalRotation.transform.right * speed / 10.0f, ForceMode.Impulse);
                        //transform.localPosition += horizontalRotation.transform.right * speed * Time.deltaTime;
                    }

                    if (grounded && Input.GetKeyDown(KeyCode.Space) && haslegs)
                    {
                        rb.AddForce(new Vector3(0, 7, 0), ForceMode.Impulse);
                    }

                    if (gameObject.GetComponent<Rigidbody>().velocity.magnitude > 0)
                    {
                        Quaternion temp = horizontalRotation.transform.rotation;
                        gameObject.transform.LookAt(gameObject.transform.position + horMov, new Vector3(0, 1, 0));
                        horizontalRotation.transform.rotation = temp;
                    }

                    GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x * 0.95f,
                        GetComponent<Rigidbody>().velocity.y,
                        GetComponent<Rigidbody>().velocity.z * 0.95f);


                    ///dash logic
                    dashtimer -= Time.deltaTime;
                    if (dashtimer < 0)
                    {
                        if (Input.GetKeyDown(KeyCode.LeftShift))
                        {
                            dashing = true;
                            dashtimer = dashduration;
                            direction.x = Input.GetAxisRaw("Horizontal");
                            direction.y = Input.GetAxisRaw("Vertical");

                            gameObject.GetComponent<MeshRenderer>().material = dashcolour;

                            dashDirection = horizontalRotation.transform.right * direction.x + horizontalRotation.transform.forward * direction.y;
                        }
                    }
                }
                else
                {
                    dashtimer -= Time.deltaTime;
                    if (dashtimer > 0)
                    {
                        gameObject.GetComponent<Rigidbody>().velocity = dashDirection * dashspeed;
                    }
                    else
                    {
                        gameObject.GetComponent<MeshRenderer>().material = regularcolour;
                        gameObject.GetComponent<Rigidbody>().velocity = new Vector3();
                        dashtimer = dashcooldown;
                        dashing = false;
                    }
                }
            }
            else
            {
                stunduration -= Time.deltaTime;
                if (stunduration <= 0)
                {
                    stunned = false;
                }
            }
        }
    }

    public void lockMovement()
    {
        unlocked = false;
    }

    public void unlockMovement()
    {
        unlocked = true;
    }

    public void Stun(float dur)
    {
        stunduration = dur;
        stunned = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HeadLaser>() != null && !dashing)
        {
            gameObject.GetComponent<PlayerStats>().takeDamage(other.GetComponent<HeadLaser>().getDamage());
            other.GetComponent<HeadLaser>().Explode();
        }
    }

    private void OnTriggerExit(Collider other)
    {

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
    }

 public bool getGrounded()
    {
        return grounded;
    }
}
