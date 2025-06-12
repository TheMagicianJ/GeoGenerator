using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    readonly Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    Color[] colours;
    public Gradient gradient;

    float maxNoiseHeight;
    float minNoiseHeight;
    public Terrain[] regions;



    void Start()
    {
        GetComponent<MeshFilter>().mesh = mesh;
        UpdateMesh();

    }


    public Mesh UpdateMesh()
    {

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.colors = colours;
        return mesh;

    }

    public void CreateGrid(int width, int height, float[,] heightMap, float heightMultiplier, AnimationCurve multiplierCurve, int detail)
    {

        int mapWidth = width;
        int mapHeight = height;

        int meshIncrementation = detail * 2;

        if (meshIncrementation == 0){

            meshIncrementation = 1;
        }

        int verticesWidth = (mapWidth-1)/meshIncrementation + 1;
        int verticesHeight = (mapHeight - 1)/meshIncrementation + 1;

        vertices = new Vector3[verticesWidth*verticesHeight];

        int index = 0;

            for (int k = 0; k <= verticesHeight - 1; k ++)
            {

                for (int i = 0; i <= verticesWidth - 1; i ++)
                {
                    float j = multiplierCurve.Evaluate(heightMap[i, k]) * heightMultiplier;
                    vertices[index] = new Vector3(i, j, k);

                    if (j > maxNoiseHeight)
                    {
                        maxNoiseHeight = j;
                    }
                    else if (j < minNoiseHeight)
                    {
                        minNoiseHeight = j;
                    }

                    index++;

                }
            }
        

        int vertex = 0;
        int tris = 0;
        triangles = new int[(verticesWidth - 1) * (verticesHeight - 1) * 6];

        for (int z = 0; z < mapHeight - 1; z++)
        {
            for (int x = 0; x < mapWidth - 1; x++)
            {

                triangles[tris] = vertex;
                triangles[tris + 1] = vertex + verticesWidth;
                triangles[tris + 2] = vertex + 1;
                triangles[tris + 3] = vertex + 1;
                triangles[tris + 4] = vertex + verticesWidth;
                triangles[tris + 5] = vertex + verticesWidth + 1;

                vertex++;
                tris += 6;

            }
            vertex++;
        }

        colours = new Color[vertices.Length];

        for (int i = 0, z = 0; z < mapHeight - 1; z++)
        {
            for (int x = 0; x < mapWidth - 1; x++)
            {

                float nheight = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, vertices[i].y);
                for(int r = 0; r < regions.Length; r++){

                    if( nheight < regions[r].height){

                        colours[i] = regions[r].colour;
                        break;

                    }
                }
                //colours[i] = gradient.Evaluate(height);
                i++;

            }
        }

    }

    public void Update()
    {

        GetComponent<MeshFilter>().mesh = mesh;


    }

    public void OnDrawGizmos()
    {

        if (vertices == null)
        {
            return;
        }
        else
        {

            for (int i = 0; i < vertices.Length; i++)
            {

                Gizmos.color = new Color(0, 0, 1, 1);
                Gizmos.DrawCube(vertices[i], new Vector3(0.1f, 0.1f, 0.1f));

            }
        }
    }
}

[System.Serializable]
public struct Terrain
{
    public string name;
    public float height;
    public Color colour;
}



