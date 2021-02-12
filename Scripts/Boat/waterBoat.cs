using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterBoat : MonoBehaviour
{
    //visible Properties
    public Transform Motor;
    public float SteerPower = 500f;
    public float Power = 5f;
    public float MaxSpeed = 10f;
    public float Drag = 0.1f;

    //used Components
    protected Rigidbody Rigidbody;
    protected Quaternion StartRotation;
    protected ParticleSystem ParticleSystem;
    protected Camera Camera;
    protected float CameraOffsetX = 0;
    protected float CameraOffsetY = 2;
    protected float CameraOffsetZ = 4;

    //internal Properties
    protected Vector3 CamVel;


    public void Awake()
    {
        ParticleSystem = GetComponentInChildren<ParticleSystem>();
        Rigidbody = GetComponent<Rigidbody>();
        StartRotation = Motor.localRotation;
        Camera = Camera.main;
    }

    public void FixedUpdate()
    {
        //default direction
        var forceDirection = transform.forward;
        var steer = 0;

        //steer direction [-1,0,1]
        if (Input.GetKey(KeyCode.A))
            steer = 1;
        if (Input.GetKey(KeyCode.D))
            steer = -1;


        //Rotational Force
        Rigidbody.AddForceAtPosition(steer * transform.right * SteerPower / 100f, Motor.position);

        //compute vectors
        var forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);
        var targetVel = Vector3.zero;

        //forward/backward poewr
        if (Input.GetKey(KeyCode.W))
            PhysicsHelper.ApplyForceToReachVelocity(Rigidbody, forward * MaxSpeed, Power);
        if (Input.GetKey(KeyCode.S))
            PhysicsHelper.ApplyForceToReachVelocity(Rigidbody, forward * -MaxSpeed, Power);

        //Motor Animation // Particle system
        Motor.SetPositionAndRotation(Motor.position, transform.rotation * StartRotation * Quaternion.Euler(0, 30f * steer, 0));
        if (ParticleSystem != null)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                ParticleSystem.Play();
            else
                ParticleSystem.Stop();
        }

        //moving forward
        var movingForward = Vector3.Cross(transform.forward, Rigidbody.velocity).y < 0;

        //move in direction
        Rigidbody.velocity = Quaternion.AngleAxis(Vector3.SignedAngle(Rigidbody.velocity, (movingForward ? 1f : 0f) * transform.forward, Vector3.up) * Drag, Vector3.up) * Rigidbody.velocity;
   
        //camera position
        Vector3 CameraOffsetPos = new Vector3(CameraOffsetX, CameraOffsetY, CameraOffsetZ);
        Vector3 CameraNewpos = transform.position + CameraOffsetPos;
        CameraNewpos.y = Mathf.Clamp(transform.position.y, 10, 2);
        Camera.transform.LookAt(CameraNewpos + transform.forward * 6f + transform.up * 2f);
        Camera.transform.position = Vector3.SmoothDamp(Camera.transform.position, transform.position + transform.forward * -8f + transform.up * 2f, ref CamVel, 0.05f);

    }

}