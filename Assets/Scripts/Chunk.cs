using System.Collections;
using UnityEngine;

public class Chunk
{
   
    public Block[,,] chunkdata;
    public GameObject goChunk;
    public readonly int chunkSize;

    readonly Material material;


    public Chunk(Vector3 pos, Material material, int numberOfChunks)
    {
        this.material = material;

        goChunk = new GameObject(CreateChunkName(pos));
        goChunk.transform.position = pos;

        chunkSize = numberOfChunks;

        BuildChunk();
    }


    public void DrawChunk()
    {
        for (int z = 0; z < chunkSize; z++)
            for (int y = 0; y < chunkSize; y++)
                for (int x = 0; x < chunkSize; x++)
                    chunkdata[x, y, z].Draw();

        CombineBlocks();
    }


    void BuildChunk()
    {
        chunkdata = new Block[chunkSize, chunkSize, chunkSize];

        for (int z = 0; z < chunkSize; z++)
            for (int y = 0; y < chunkSize; y++)
                for (int x = 0; x < chunkSize; x++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    
                    if(Random.Range(0f,1f) < 0.5f)
                    {
                        chunkdata[x, y, z] = new Block(Block.BlockType.GRASS, pos, this, material);
                    }
                    else
                    {
                        chunkdata[x, y, z] = new Block(Block.BlockType.AIR, pos, this, material);

                    }
                }
    }


    void CombineBlocks()
    {
        MeshFilter[] childMeshFilters = goChunk.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combinedChildMeshes = new CombineInstance[childMeshFilters.Length];

        int i = 0;
        while (i < childMeshFilters.Length)
        {
            combinedChildMeshes[i].mesh = childMeshFilters[i].sharedMesh;
            combinedChildMeshes[i].transform = childMeshFilters[i].transform.localToWorldMatrix;
            i++;
        }

        MeshFilter parentMeshFilter = goChunk.AddComponent<MeshFilter>();
        parentMeshFilter.mesh.CombineMeshes(combinedChildMeshes);
        MeshRenderer parentRenderer = goChunk.AddComponent<MeshRenderer>();
        parentRenderer.material = material;

        foreach (Transform block in goChunk.transform)
        {
            GameObject.Destroy(block.gameObject);
        }

    }

    public static string CreateChunkName(Vector3 v)
    {
        return (int)v.x + " " + (int)v.y + " " + (int)v.z + " ";
    }
}
