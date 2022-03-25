using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseGenerator 
{
    public static float[,] NoiseMap(int width, int height, float scale, int seed)
    {
        float[,] noiseMap = new float[width, height];

        System.Random prng = new System.Random(seed);

        float offestX = prng.Next(-10000, 10000);
        float offestY = prng.Next(-10000, 10000);

        if(scale <= 0)
        {
            scale = 0.0001f;
        }

        for(int y = 0; y < height; y ++)
        {
            for(int x = 0; x < width; x ++)
            {
                float sampleX = x / scale + offestX;
                float sampleY = y / scale + offestY;

                float  perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                noiseMap[x,y] = perlinValue;
            }
        }
        
        return noiseMap;
    }
}
