using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Size")]
    public const int mapChunkSize = 145;
    public int width = 1;
    public int height = 1;

    [Range(0,6)]
    public int detail;


    [Header("Noise Stats")]
    public float noiseScale;
    public int octaves;

    [Range(0,1)]
    public float persistence;
    
    public float lacunarity;
    public int seed;
    float[,] noiseMap;

    [Header("Mesh")]
    MeshGenerator meshGenerator;
    public float heightMultiplier;
    public AnimationCurve multiplierCurve;





    public void GenerateMap(){

        float[,] noiseMap = Noise.CreateNoiseMap( seed, width , height,noiseScale, octaves, persistence, lacunarity);
        meshGenerator = FindObjectOfType<MeshGenerator>();
        meshGenerator.CreateGrid(noiseMap,heightMultiplier, multiplierCurve, detail);
        meshGenerator.UpdateMesh();
    }


    private void Start(){

         GenerateMap();
    }

    private void Update(){

    
         GenerateMap();

         
    }

    #if UNITY_EDITOR
    private void OnValidate() {

        

    
         if(lacunarity < 1){
            
            lacunarity = 1;
         }
         if(octaves < 0){

            octaves = 0;
         }

         GenerateMap();


    }

    #endif

}
