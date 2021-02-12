using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    protected waterBoat Boatdata;
    protected float speed;
    protected Vector3 PlayerNewPos;
    protected Vector3 PlayerRot;

    // Start is called before the first frame update
    void Start()
    {
        Boatdata = GameObject.FindGameObjectWithTag("Boat").GetComponent<waterBoat>();
        speed = Boatdata.MaxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //PlayerRot = transform.eulerAngles;
        Vector3 PlayerOffset = new Vector3(0, 0, -6.5f);
        PlayerNewPos = Boatdata.transform.position + PlayerOffset;
        transform.position = Vector3.MoveTowards(transform.position, PlayerNewPos, speed * Time.deltaTime);
        //PlayerRot.z = Mathf.Clamp(PlayerRot.z, 0, 0);

    }
}
