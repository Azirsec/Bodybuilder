using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLooking : MonoBehaviour
{
    public GameObject horizontalRotation;
    public GameObject verticalRotation;
    float rotationSpeed = 30;

    float maxRotation = 60;
    float minRotation = -60;
    float currentVerticalRotation;

    bool unlocked = false;

    // Start is called before the first frame update
    void Start()
    {
        currentVerticalRotation = 0;

        unlockLooking();
    }

    // Update is called once per frame
    void Update()
    {
        if (unlocked)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                horizontalRotation.transform.Rotate(new Vector3(0, -1, 0) * rotationSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                horizontalRotation.transform.Rotate(new Vector3(0, 1, 0) * rotationSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                currentVerticalRotation += rotationSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                currentVerticalRotation += -rotationSpeed * Time.deltaTime;
            }

            currentVerticalRotation -= Input.GetAxis("Mouse Y");

            //currentVerticalRotation = Mathf.Clamp(currentVerticalRotation, Mathf.Max(minRotation, currentVerticalRotation), Mathf.Min(maxRotation, currentVerticalRotation));
            currentVerticalRotation = Mathf.Clamp(currentVerticalRotation, minRotation, maxRotation);

            verticalRotation.transform.localRotation = Quaternion.Euler(currentVerticalRotation, 0, 0);
            horizontalRotation.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0));
        }
    }

    public void lockLooking()
    {
        unlocked = false;
        Cursor.lockState = CursorLockMode.None;
    }

    public void unlockLooking()
    {
        unlocked = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void resetRotation()
    {
        verticalRotation.transform.localRotation = Quaternion.Euler(currentVerticalRotation, 0, 0);
        horizontalRotation.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}
