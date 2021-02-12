using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves7 : MonoBehaviour
{
    public Octave[] Octaves; // to repeat creation of mesh for waves
    public int Dimension = 10;
    public float UVscale = 2f;

    protected MeshFilter MeshFilter;
    protected Mesh Mesh;
    protected waterBoat WaveTransform;
    protected Vector3 BoatStartPosition;
    protected Vector3 BoatNewPosition;
    protected float speed;

    // Start is called before the first frame update
    void Start()
    {
        WaveTransform = GameObject.FindGameObjectWithTag("Boat").GetComponent<waterBoat>();
        BoatStartPosition = WaveTransform.transform.position;
        speed = WaveTransform.MaxSpeed;
        //Mesh Setup
        Mesh = new Mesh();
        Mesh.name = gameObject.name;

        Mesh.vertices = GenerateVerts();
        Mesh.triangles = GenerateTries();
        Mesh.uv = GenerateUVs();
        Mesh.RecalculateBounds();
        Mesh.RecalculateNormals();

        MeshFilter = gameObject.AddComponent<MeshFilter>();
        MeshFilter.mesh = Mesh;
    }

    public float GetHeight(Vector3 position) // if waves height is not 1 or 0 but a float then this will calculate the height of it
    {
        //scale factor and position in local space
        var scale = new Vector3(1 / transform.lossyScale.x, 0, 1 / transform.lossyScale.z);
        var localPos = Vector3.Scale((position - transform.position), scale);

        //get edge points
        var p1 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Floor(localPos.z));
        var p2 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Ceil(localPos.z));
        var p3 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Floor(localPos.z));
        var p4 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Ceil(localPos.z));

        //clamp if the position is outside the plane
        p1.x = Mathf.Clamp(p1.x, 0, Dimension);
        p1.z = Mathf.Clamp(p1.z, 0, Dimension);
        p2.x = Mathf.Clamp(p2.x, 0, Dimension);
        p2.z = Mathf.Clamp(p2.z, 0, Dimension);
        p3.x = Mathf.Clamp(p3.x, 0, Dimension);
        p3.z = Mathf.Clamp(p3.z, 0, Dimension);
        p4.x = Mathf.Clamp(p4.x, 0, Dimension);
        p4.z = Mathf.Clamp(p4.z, 0, Dimension);

        //get the max distance to one of the edges and take that to compute max - dist
        var max = Mathf.Max(Vector3.Distance(p1, localPos), Vector3.Distance(p2, localPos), Vector3.Distance(p3, localPos), Vector3.Distance(p4, localPos) + Mathf.Epsilon);
        var dist = (max - Vector3.Distance(p1, localPos))
                 + (max - Vector3.Distance(p2, localPos))
                 + (max - Vector3.Distance(p3, localPos))
                 + (max - Vector3.Distance(p4, localPos) + Mathf.Epsilon);
        //weighted sum
        var height = Mesh.vertices[index(p1.x, p1.z)].y * (max - Vector3.Distance(p1, localPos))
                   + Mesh.vertices[index(p2.x, p2.z)].y * (max - Vector3.Distance(p2, localPos))
                   + Mesh.vertices[index(p3.x, p3.z)].y * (max - Vector3.Distance(p3, localPos))
                   + Mesh.vertices[index(p4.x, p4.z)].y * (max - Vector3.Distance(p4, localPos));

        //scale
        return height * transform.lossyScale.y / dist;


    }

    private Vector2[] GenerateUVs() // texture map to actual wireframe of waves asign the pixels for each vertices in the wireframes ... we flip it so the next verts right to it is opposite from previous
    {
        var uvs = new Vector2[Mesh.vertices.Length]; // we need to specify which vertices the uvs have

        //always set ome uv over n tiles then flip the uv and set it again
        for (int x = 0; x <= Dimension; x++)
        {
            for (int z = 0; z <= Dimension; z++)
            {
                var vec = new Vector2((x / UVscale) % 2, (z / UVscale)); // gives us the modular of the 2 when x/uvscale is a certain value then result = 1 like truth tables
                uvs[index(x, z)] = new Vector2(vec.x <= 1 ? vec.x : 2 - vec.x, vec.y <= 1 ? vec.y : 2 - vec.y); // if result = 0 then we take 0 if not then we take 2 - answer for x and y so we always get an answer of 1 or 0
            } // so it altermates from 0 - 1 from every vert to the next
        }
        return uvs;
    }

    private Vector3[] GenerateVerts()
    {
        var verts = new Vector3[(Dimension + 1) * (Dimension + 1)]; //11 by 11 area

        //equally distributed verts
        for (int x = 0; x <= Dimension; x++) // repeats 11x same with z
            for (int z = 0; z <= Dimension; z++)
                verts[index(x, z)] = new Vector3(x, 0, z);

        return verts;
    }
    private int index(float x, float z)//test
    {
        return (int)(x * (Dimension + 1) + z);
    }

    private int[] GenerateTries()
    {
        var tries = new int[Mesh.vertices.Length * 6];

        //equally distribute tri's
        for (int x = 0; x < Dimension; x++)
        {
            for (int z = 0; z < Dimension; z++)
            {
                tries[index(x, z) * 6 + 0] = index(x, z); //v1 to do the same logic for tries on each index u times by 6 
                tries[index(x, z) * 6 + 1] = index(x + 1, z + 1); //coordinates for v3
                tries[index(x, z) * 6 + 2] = index(x + 1, z); //v2
                tries[index(x, z) * 6 + 3] = index(x, z); //v4
                tries[index(x, z) * 6 + 4] = index(x, z + 1); //v6
                tries[index(x, z) * 6 + 5] = index(x + 1, z + 1); //v5

            }
        }
        return tries;
    }

    private void FixedUpdate()
    {
        BoatNewPosition = WaveTransform.transform.position;
        Vector3 WavePosOffset = new Vector3(-20, -1.5f, -20);
        Vector3 WaveNewPosition = BoatNewPosition + WavePosOffset;
        //if (BoatStartPosition != BoatNewPosition)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, WaveNewPosition, speed * Time.deltaTime);
        //}
    }

    // Update is called once per frame
    void Update()
    {


        var verts = Mesh.vertices; //wrap the vertices into new variable to equally distribute 
        for (int x = 0; x <= Dimension; x++)
        {
            for (int z = 0; z <= Dimension; z++)
            {
                var y = 0f;
                for (int o = 0; o < Octaves.Length; o++)
                {
                    if (Octaves[o].alternate) //if conditions met then does the alternate animation
                    {
                        var perl = Mathf.PerlinNoise((x * Octaves[o].scale.x) / Dimension, (z * Octaves[o].scale.y) / Dimension) * Mathf.PI * 2f; //add a factor of Perlin Noise (basically (x * z) / y but you need to scale)
                        y += Mathf.Cos(perl + Octaves[o].speed.magnitude * Time.time) * Octaves[o].height; // to create alternating anim  => cos wave * amount of speed * time * height ... the added perl noise makes it so we get different heights based on our x and z coords
                    }
                    else
                    {
                        var perl = Mathf.PerlinNoise((x * Octaves[o].scale.x + Time.time * Octaves[o].speed.x) / Dimension, (z * Octaves[o].scale.y + Time.time * Octaves[o].speed.y) / Dimension) - 0.5f; //used z and y for scale octave cause speed and scale is set to vector 2 also - 0.5 cause perl noise gives us a value of 0-1 so to get the waves from its peak instead of base you subtract by 0.5
                        y += perl * Octaves[o].height; //instead of adding the perl noise you just times with octave height instead and no more time its up in the line above to make the waves go in a straight line
                    }
                }

                verts[index(x, z)] = new Vector3(x, y, z);
            }
        }
        Mesh.vertices = verts; // then put it back together
        Mesh.RecalculateNormals();
    }

    [Serializable]
    public struct Octave
    {
        public Vector2 speed;
        public Vector2 scale;
        public float height;
        public bool alternate;
    }
}
