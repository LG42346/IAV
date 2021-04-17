using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{

    float t;
    float inc = 0.01f;

    static float smooth;
    //static int maxHeight = 255;
    static int maxHeight = 40;
    static int octaves = 6;
    static float persistence = 0.7f;

    public static int GenerateHeight(float x, float z)
    {
        return (int)Map(0, maxHeight, 0, 1, FBM(x * smooth, z * smooth, octaves, persistence));
    }

    static float Map(float newMin, float newMax, float oMin, float oMax, float currentVal)
    {
        return Mathf.Lerp(newMin, newMax, Mathf.InverseLerp(oMin, oMax, currentVal));
    }

    float FBM(float t, int octaves, float persistence)
    {
        float total = 0;
        float amplitude = 1;
        float frequency = 1;
        float maxValue = 0;

        for (int i = 0; i < octaves; i++)
        {
            total += Mathf.PerlinNoise(t, 1) * amplitude;
            amplitude *= persistence;
            frequency *= 2;
            maxValue += amplitude;
        }

        return total / maxValue;
    }
}
