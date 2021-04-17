using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorldOfBlocks : MonoBehaviour
{
    [SerializeField]
    Material material;
    [SerializeField]
    //int worldSize = 4;
    int worldSize = 2;
    [SerializeField]
    int columnHeight = 4;
    [SerializeField]
    int chunkSize = 16;

    public static Dictionary<string, Chunk> chunkDict;


    void Start()
    {
        chunkDict = new Dictionary<string, Chunk>();

        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
        
        StartCoroutine(BuildWorld());
    }


    IEnumerator BuildWorld()
    {
        for(int z=0; z < worldSize; z++)
            for (int x = 0; x < worldSize; x ++)
                for (int y = 0; y < columnHeight; y ++)
                {
                    Vector3 chunkPos = new Vector3(x*chunkSize, y*chunkSize, z*chunkSize);
                    Chunk c = new Chunk(chunkPos, material, chunkSize);
                    c.goChunk.transform.parent = transform;
                    chunkDict.Add(c.goChunk.name, c);
                }

        //We add to dict before drawing to solve neighbour issues
        foreach (KeyValuePair<string, Chunk> c in chunkDict)
        {
            c.Value.DrawChunk();
            yield return null;
        }

    }


    //IEnumerator BuildChunkColumn()
    //{
    //    for (int i = 0; i < columnHeight; i++)
    //    {
    //        Vector3 chunkPos = new Vector3(
    //            this.transform.position.x, i * chunkSize + transform.position.z);

    //        Chunk c = new Chunk(chunkPos, material, chunkSize);
    //        c.goChunk.transform.parent = this.transform;

    //        chunkDict.Add(c.goChunk.name, c);
    //    }

    //    foreach (KeyValuePair<string, Chunk> c in chunkDict)
    //    {
    //        c.Value.DrawChunk();
    //        yield return null;
    //    }
    //}


}
