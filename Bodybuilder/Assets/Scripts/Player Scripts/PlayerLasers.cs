using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLasers : MonoBehaviour
{

    [SerializeField] Image reticle;

    [SerializeField] Transform leftEyePos;
    [SerializeField] Transform rightEyePos;

    [SerializeField] Transform head;

    [SerializeField] private GoodLaser laser;

    [SerializeField] Camera camera;

    private Vector3 aimlocation;

    float sightdist = 15;

    float cooldown = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;
        if (Input.GetMouseButton(1))
        {
            RaycastHit hit1;
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit1, sightdist))
            {
                aimlocation = hit1.collider.gameObject.transform.position;
            }
            else
            {
                aimlocation = camera.transform.position + camera.transform.forward * sightdist;
            }
            //make reticle visible
            reticle.enabled = true;
            
            head.LookAt(aimlocation);
            head.Rotate(new Vector3(-90, 0, 0));

            if (Input.GetMouseButton(0))
            {
                if (cooldown < 0)
                {
                    FireLasers();
                    cooldown = 2;
                }
            }
        }
        else
        {
            //make reticle invisible
            reticle.enabled = false;
            head.LookAt(transform.position + transform.forward);
            head.Rotate(new Vector3(-120, 0, 0));
        }
    }

    void FireLasers()
    {
        //do a raycast from the camera position and direction, then either where it collides or 
        //at the end of its sightdist that is where aimlocation is

        Quaternion direction = new Quaternion();
        direction.SetLookRotation(aimlocation - rightEyePos.position);
        Instantiate(laser, rightEyePos.position + camera.transform.forward, direction);

        direction.SetLookRotation(aimlocation - leftEyePos.position);
        Instantiate(laser, leftEyePos.position + camera.transform.forward, direction);
    }
}
