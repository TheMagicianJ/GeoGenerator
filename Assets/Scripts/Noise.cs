using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] CreateNoiseMap(int seed, int mapWidth, int mapHeight, float noiseScale, int octaves, float persistence, float lacunarity)
    {

        float[,] noiseMap = new float[mapWidth, mapHeight];
        System.Random prng = new System.Random(seed);
        Vector2[] offsetsOctave = new Vector2[ octaves];

        for (int i = 0; i < octaves; i++){
            
            float offsetX = prng.Next (-100000,100000);
            float offSetY = prng.Next(-100000, 100000);

            offsetsOctave[i] = new Vector2(offsetX, offSetY);
        }
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        if (noiseScale <= 0){

            noiseScale = 0.0001f;
        }


        for (int y = 0; y < mapHeight; y++){

            for (int x = 0; x < mapWidth; x++){

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int o = 0; o < octaves; o++){

                    float sampleOfX = ((x / noiseScale) * frequency) + offsetsOctave[o].x;
                    float sampleOfY = ((y / noiseScale) * frequency) + offsetsOctave[o].y;
                    float perlinVal = (Mathf.PerlinNoise(sampleOfX, sampleOfY)*2) -1;
                    noiseHeight += perlinVal * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }
                
                if (noiseHeight > maxNoiseHeight){
                    maxNoiseHeight = noiseHeight;
                }
                else if(noiseHeight < minNoiseHeight){
                    minNoiseHeight = noiseHeight;
                }
                
                noiseMap[x,y] = noiseHeight;

            }
        }

        for (int y = 0; y < mapHeight; y++){
        
            for (int x = 0; x < mapWidth; x++){

                noiseMap[x,y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x,y]);



            }
        }
        return noiseMap;

    }

}
