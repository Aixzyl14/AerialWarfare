using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappyCube : MonoBehaviour
{
    protected MeshFilter MeshFilter;
    protected Mesh Mesh;

    public float upDownFactor = 0.1f;
    public float upDownSpeed = 6f;
    public float leftFactor = 0.3f;
    public float leftSpeed = 3f;
    public float leftOffset = 2.3f;
    public float stretchFactor = -0.1f;
    public float stretchSpeed = 6f;

    // Start is called before the first frame update
    void Start()
    {
        Mesh = new Mesh();
        Mesh.name = "GeneratedMesh";

        Mesh.vertices = GenerateVerts();
        Mesh.triangles = GenerateTriangles();

        Mesh.RecalculateNormals(); // vectors that stand on side of vertices
        Mesh.RecalculateBounds(); //boundary boxset that contains all vertices

        MeshFilter = gameObject.AddComponent<MeshFilter>();
        MeshFilter.mesh = Mesh;
    }

    private Vector3[] GenerateVerts(float up = 0f, float left = 0f, float stretch = 0f)
    {
        return new Vector3[]
        {
            //top of cube vertices
            new Vector3(-1, 0 ,1),
            new Vector3(1 ,0,1),
            new Vector3(1,0,-1 ),
            new Vector3(-1,0,-1 ),

            //bottom of cube vertices
            new Vector3(-1 - stretch + left,2 + up,1 + stretch),
            new Vector3(1 + stretch + left,2 + up,1 + stretch),
            new Vector3(1 + stretch + left,2 + up,-1 - stretch),
            new Vector3(-1 - stretch + left,2 + up,-1 - stretch),


            //left of cube vertices
            new Vector3(-1,0,1),
            new Vector3(-1,0,-1),
            new Vector3(-1 - stretch + left,2 + up,1 + stretch),
            new Vector3(-1 - stretch + left,2 + up,-1 - stretch),

            //right of cube vertices
            new Vector3(1 ,0,1),
            new Vector3(1 ,0,-1 ),
            new Vector3(1 + stretch + left,2 + up,1 + stretch),
            new Vector3(1 + stretch + left,2 + up,-1 - stretch),

            //front of cube vertices
            new Vector3(1,0,-1),
            new Vector3(-1,0,-1),
            new Vector3(1 + stretch + left,2 + up,-1 - stretch),
            new Vector3(-1 - stretch + left,2 + up,-1 - stretch),

            //back of cube vertices
             new Vector3(-1,0,1),
            new Vector3(1,0,1),
            new Vector3(-1 - stretch + left,2 + up,1 + stretch),
            new Vector3(1 + stretch + left,2 + up,1 + stretch),


        };
    }

    private int[] GenerateTriangles()
    {
        return new int[]
        {
            //bottom/top of cube triangles
            1,0,2,
            2,0,3,
            4,5,6,
            4,6,7,

            //left/right of cube triangles
            9,10,11,
            8,10,9,
            12,13,15,
            14,12,15,

            //front/back of cube triangles
            16,17,19,
            18,16,19,
            20,21,23,
            22,20,23,



        };
    }


    // Update is called once per frame
    void Update()
    {
        Mesh.vertices = GenerateVerts(Mathf.Sin(Time.realtimeSinceStartup * upDownSpeed) * upDownFactor,
                                      Mathf.Sin(Time.realtimeSinceStartup * leftSpeed + leftOffset) * leftFactor,
                                      Mathf.Sin(Time.realtimeSinceStartup * stretchSpeed) * stretchFactor);
    }
}
