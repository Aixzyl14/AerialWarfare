using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerControls : MonoBehaviour
{

    [Header("General")]
    [Tooltip("In ms^-1")] [SerializeField] float ControlSpeed = 10f;
    [Tooltip("In m")] [SerializeField] float xRange = 25f;
    [Tooltip("In m")] [SerializeField] float yRange = 13f;
    [SerializeField] GameObject[] guns;

    [Header("Screen-Position Based")]
    [SerializeField] float positionPitchFactor = -1f;
    [SerializeField] float positionYawFactor = 2.5f;

    [Header("Control-Throw Based")]
    [SerializeField] float controlPitchFactor = -25f;
    [SerializeField] float controlRollFactor = -7f;

    float yThrow;
    float xThrow;

    bool isControlEnabled = true;


    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        if (isControlEnabled == true)
        {
            ProcessTranslation();
            ProcessRotation();
            ProcessFiring();
        }
    }

    private void ProcessTranslation()
    {
        xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        yThrow = CrossPlatformInputManager.GetAxis("Vertical");

        float xOffset = xThrow * ControlSpeed * Time.deltaTime;
        float yOffset = yThrow * ControlSpeed * Time.deltaTime;

        float rawXPos = transform.localPosition.x + xOffset;
        float ClampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);

        float rawYPos = transform.localPosition.y + yOffset;
        float ClampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(ClampedXPos, ClampedYPos, transform.localPosition.z);
    }

    private void OnPlayerDeath() //Call by string reference
    {
        print("Dead!");
        isControlEnabled = false;
    }

    private void ProcessRotation()
    {
       
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;
        float pitch = pitchDueToPosition + pitchDueToControlThrow;
        float yaw = transform.localPosition.x * positionYawFactor;
        float roll = xThrow * controlRollFactor;
        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

   void ProcessFiring()
    {
        if (CrossPlatformInputManager.GetButton("Fire"))
        {

            SetGunsActive(true);
        }
        else
        {
            SetGunsActive(false);
        }
    }

    private void SetGunsActive(bool isActive)
    {
        foreach(GameObject gun in guns)
        {
           var emissionModule = gun.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }



}
