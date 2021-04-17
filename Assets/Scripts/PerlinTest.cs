using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinTest : MonoBehaviour
{

    float t;
    float tt1;
    float tt2;
    float tt3;
    float inc1 = 0.001f;
    float inc2 = 0.05f;
    float inc3 = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        t += Time.deltaTime;
        float hc1 = 0.5f * (Mathf.Cos(2 * t + 0.4f) + 1);
        float hc2 = 0.25f * (Mathf.Cos(4 * t) + 1);
        float hc3 = 0.125f * (Mathf.Cos(6 * t) + 1);
        float hr = Random.Range(0, 1);

        float hp1 = Mathf.PerlinNoise(tt1, 1);
        float hp2 = Mathf.PerlinNoise(tt2, 1);
        float hp3 = Mathf.PerlinNoise(tt3, 1);

        tt1 += inc1;
        tt2 += inc2;
        tt3 += inc3;

        Grapher.Log(hp1 + hp2 + hp3, "Perlin", Color.yellow);
        Grapher.Log(hc1 + hc2+ hc3, "Harmonicas", Color.yellow);
        Grapher.Log(hc1, "Cos", Color.green);
        Grapher.Log(hr, "Random", Color.red);
        }
}
