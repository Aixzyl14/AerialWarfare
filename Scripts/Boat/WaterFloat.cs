using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFloat : MonoBehaviour
{
    //public properties
    public float AirDrag = 1;
    public float waterDrag = 10;
    public Transform[] FloatPoints;
    public bool AttachToSurface;

    //used components 
    protected Rigidbody Rigidbody;
    protected Waves Waves;

    //water line
    protected float WaterLine;
    protected Vector3[] WaterLinePoints;
    
    //help Vectors
    protected Vector3 centerOffset;
    protected Vector3 smoothVectorRotation;
    protected Vector3 Targetup;

    public Vector3 Center { get { return transform.position + centerOffset; } }


    // Start is called before the first frame update
    void Awake()
    {
        Waves = FindObjectOfType<Waves>();
        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.useGravity = false; // as we will create our own gravity rules


        //compute center
        WaterLinePoints = new Vector3[FloatPoints.Length]; //basically our float points
        for (int i = 0; i < FloatPoints.Length; i++)
            WaterLinePoints[i] = FloatPoints[i].position; //the points underwater
        centerOffset = PhysicsHelper.GetCenter(WaterLinePoints) - transform.position; //pass the points of the center to physicshelper script
    }

    // Update is called once per frame
    void Update()
    {
        //default water surface
        var newWaterLine = 0f;
        var pointUnderWater = false;

        //set WaterLinePoints and WaterLine
        for (int i = 0; i < FloatPoints.Length; i++) // go through all the floating points and check its position and see if its y axis is higher than height of waves
        {
            //height
            WaterLinePoints[i] = FloatPoints[i].position;
            WaterLinePoints[i].y = Waves.GetHeight(FloatPoints[i].position);
            newWaterLine += WaterLinePoints[i].y / FloatPoints.Length; //the new waterline is the wavesheight / floatingpoints 
            if (WaterLinePoints[i].y > FloatPoints[i].position.y) //if any of the water line points is higher than the float points than the object is under water
                pointUnderWater = true;
        }

        var waterLineDelta = newWaterLine - WaterLine;
        WaterLine = newWaterLine;

        //added gravity
        var gravity = Physics.gravity;
        Rigidbody.drag = AirDrag;
        if (WaterLine > Center.y)
        {
            Rigidbody.drag = waterDrag;
            if (AttachToSurface)
            {
                //attach to water surface
                Rigidbody.position = new Vector3(Rigidbody.position.x, WaterLine - centerOffset.y, Rigidbody.position.z);
            }
            else
            {
                //go up
                gravity = -Physics.gravity;
                transform.Translate(Vector3.up * waterLineDelta * 0.9f);
            } 
        }
        Rigidbody.AddForce(gravity * Mathf.Clamp(Mathf.Abs(WaterLine - Center.y), 0,1)); //linear factor of waves * gravity

        //compute up vector
        Targetup = PhysicsHelper.GetNormal(WaterLinePoints); //get normal of all waterline points

        //rotation of object
        if (pointUnderWater)
        {
            //attach to water surface
            Targetup = Vector3.SmoothDamp(transform.up, Targetup, ref smoothVectorRotation, 0.2f); //slowly transform the transform up vector to the target up vector rotating at 0.2f speed
            Rigidbody.rotation = Quaternion.FromToRotation(transform.up, Targetup) * Rigidbody.rotation; // complete the rotation
        }
    }
}
