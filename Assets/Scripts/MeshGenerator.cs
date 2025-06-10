using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
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

    }


    public Mesh UpdateMesh()
    {

        
        mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.colors = colours;
        return mesh;

    }

    public void CreateGrid(float[,] heightMap, float heightMultiplier, AnimationCurve multiplierCurve, int detail)
    {

        int mapWidth = heightMap.GetLength(0);
        int mapHeight = heightMap.GetLength(1);

        int meshIncrementation = detail * 2;

        if (meshIncrementation == 0){

            meshIncrementation = 1;
        }

        int verticesPerLine = (((mapWidth-1)/(meshIncrementation) + 1));

        vertices = new Vector3[verticesPerLine*verticesPerLine];

        int index = 0;
        for (int k = 0; k <= mapHeight - 1; k += meshIncrementation)
        {

            for (int i = 0; i <= mapWidth - 1; i += meshIncrementation)
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
        triangles = new int[(verticesPerLine - 1) * (verticesPerLine - 1) * 6];
        for (int z = 0; z < mapHeight - 1; z++)
        {
            for (int x = 0; x < mapWidth - 1; x++)
            {

                triangles[tris] = vertex;
                triangles[tris + 1] = vertex + verticesPerLine;
                triangles[tris + 2] = vertex + 1;
                triangles[tris + 3] = vertex + 1;
                triangles[tris + 4] = vertex + verticesPerLine;
                triangles[tris + 5] = vertex + verticesPerLine + 1;

                vertex++;
                tris = tris + 6;

            }
            vertex++;
        }

        colours = new Color[vertices.Length];

        for (int i = 0, z = 0; z < mapHeight - 1; z++)
        {
            for (int x = 0; x < mapWidth - 1; x++)
            {

                float height = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, vertices[i].y);
                for(int r = 0; r < regions.Length; r++){

                    if( height < regions[r].height){

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



