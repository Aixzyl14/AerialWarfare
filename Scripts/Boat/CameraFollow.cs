using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    protected waterBoat Boatdata;
    protected Vector3 BoatNewPos;
    protected float BoatOrigRot;
    protected float BoatNewRot;
    protected float speed;

    protected float CameraOffsetX = 0;
    protected float CameraOffsetY = 20;
    protected float CameraOffsetZ = -18;

    protected float CameraRotOffsetX = 20.8f;
    protected float CameraRotOffsetY = 8;
    protected float CameraRotOffsetZ = 0;
    float offsetaddonX = 5;
    float offsetaddonZ = 5;
    protected bool CameraAlligned = false;
    protected bool halfRot = false;

    enum Processes{CameraAlligned, CameraRot };
    Processes processes = Processes.CameraRot;
    private void Start()
    {
        Boatdata = GameObject.FindGameObjectWithTag("Boat").GetComponent<waterBoat>();
        speed = Boatdata.MaxSpeed;
    }

    private void FixedUpdate()
    {
        Vector3 CameraOffsetPos = new Vector3(CameraOffsetX, CameraOffsetY, CameraOffsetZ);
        BoatNewPos = Boatdata.transform.position + CameraOffsetPos;
        transform.position = Vector3.MoveTowards(transform.position, BoatNewPos, speed * Time.deltaTime);
        BoatNewRot = Boatdata.transform.eulerAngles.y;
        transform.eulerAngles = new Vector3(CameraRotOffsetX, BoatNewRot, CameraRotOffsetZ);
        
        //if ((BoatNewRot <= 1) && CameraAlligned == false)
        //{
        //    BoatOrigRot = Boatdata.transform.eulerAngles.y;
        //    print("HI");
        //    processes = Processes.CameraAlligned;
        //    CameraAlligned = true;
        //}
        //print(BoatNewRot);
        //if (processes == Processes.CameraAlligned)
        //{
        //    if ((BoatNewRot >= 178) && halfRot == false)
        //    {
        //        CameraOffsetX = 0;
        //        CameraOffsetY = 8;
        //        CameraOffsetZ = 10;
        //        CameraRotOffsetX = 20.8f;
        //        CameraRotOffsetY = 8;
        //        CameraRotOffsetZ = 0;
        //        halfRot = true;
        //        BoatNewPos = Boatdata.transform.position + CameraOffsetPos;
        //        transform.position = Vector3.MoveTowards(transform.position, BoatNewPos, speed * Time.deltaTime);
        //        BoatNewRot = Boatdata.transform.eulerAngles.y;
        //        transform.eulerAngles = new Vector3(CameraRotOffsetX, BoatNewRot, CameraRotOffsetZ);
        //    }
        //    if (BoatNewRot > (BoatOrigRot + 30))
        //    {
        //        CameraOffsetX -= offsetaddonX;
        //        CameraOffsetZ += offsetaddonZ;
        //        CameraRotOffsetX = Boatdata.transform.eulerAngles.x;
        //        CameraRotOffsetZ = Boatdata.transform.eulerAngles.z;
        //        print("bob");
        //        BoatOrigRot += 30;
        //        offsetaddonX -= 2f;
        //        offsetaddonZ -= 0.25f ;

        //    }
        //}
    }
}
