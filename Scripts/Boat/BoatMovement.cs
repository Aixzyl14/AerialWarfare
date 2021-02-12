using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    protected float CameraRotx;
    protected float CameraRoty;
    protected float CameraRotz;
    public float CamRotOffsetx;
    public float CamRotOffsety;
    public float CamRotOffsetz;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CameraRotx = FindObjectOfType<Camera>().transform.eulerAngles.x;
        CameraRoty = FindObjectOfType<Camera>().transform.eulerAngles.y;
        CameraRotz = FindObjectOfType<Camera>().transform.eulerAngles.z;
        float CameraNewRotx = CameraRotx + CamRotOffsetx;
        float CameraNewRoty = Mathf.Clamp((CameraRoty + CamRotOffsety), 0, 40);
        
        float CameraNewRotz = CameraRotz + CamRotOffsetz;
        print(CameraNewRoty);
        transform.localRotation = Quaternion.Euler(CameraNewRotx ,CameraNewRoty ,CameraNewRotz );
        
    }
}
