using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    enum Cubeside { FRONT, BACK, TOP, BOTTOM, LEFT, RIGHT }
    enum CubesideRender{ TOP, BOT, SIDE, ALL}
    public enum BlockType { GRASS, DIRT, STONE, AIR };

    //how does readonly work?
    readonly bool isSolid = true;
    readonly BlockType choosenBlockType;
    readonly Material material;
    readonly Chunk owner;

    Vector3 origin;
   
    static Vector2 GrassSide_LBC = new Vector2(3f, 15f) / 16;
    static Vector2 GrassTop_LBC = new Vector2(2f, 6f) / 16;
    static Vector2 Dirt_LBC = new Vector2(2f, 15f) / 16;
    static Vector2 Stone_LBC = new Vector2(0f, 14f) / 16;

    //Vector2[] LBCs = { GrassSide_LBC, GrassTop_LBC, Dirt_LBC, Stone_LBC };
    readonly Dictionary<(BlockType, CubesideRender), Vector2> uvBlockByType = new Dictionary<(BlockType, CubesideRender), Vector2>
    {
        { (BlockType.GRASS, CubesideRender.BOT),Dirt_LBC },
        { (BlockType.GRASS, CubesideRender.TOP),GrassTop_LBC },
        { (BlockType.GRASS, CubesideRender.SIDE),GrassSide_LBC },
        { (BlockType.DIRT, CubesideRender.ALL),Dirt_LBC },
        { (BlockType.STONE, CubesideRender.ALL),Stone_LBC },

    };

    //uvBlockByType.Add((BlockType.GRASS, CubesideRender.BOT), Dirt_LBC);
    //uvBlockByType.Add((BlockType.GRASS, CubesideRender.TOP), GrassTop_LBC);
    //uvBlockByType.Add((BlockType.GRASS, CubesideRender.SIDE), GrassSide_LBC);
    //uvBlockByType.Add((BlockType.DIRT, CubesideRender.ALL), Dirt_LBC);
    //uvBlockByType.Add((BlockType.STONE, CubesideRender.ALL), Stone_LBC);


    public Block(BlockType blockType, Vector3 position, Chunk owner, Material spriteAtlas)
    {
        this.choosenBlockType = blockType;
        this.origin = position;
        this.owner = owner;
        this.material = spriteAtlas;

        if(choosenBlockType == BlockType.AIR)
        {
            isSolid = false;
        }

      
    }


    Vector2[] GenerateBlockUV(Vector2 currentTextureLBC)
    {
        Vector2[] currentTexture =
            {
            currentTextureLBC + new Vector2(1f,1f)/16,
            currentTextureLBC + new Vector2(0f,1f)/16,
            currentTextureLBC,
            currentTextureLBC + new Vector2(1f,0f)/16
            };

        return currentTexture;
    }


    int ConvertToLocalIndex(int coordinate)
    {
        if (coordinate == -1)
            return owner.chunkSize -1;
        if (coordinate == owner.chunkSize)
            return 0;
        return coordinate;
    }

    //Receives neighbour position and checks if its solid and if its in another chunk 
    bool HasSolidNeighbour(int x, int y, int z)
    {
        Block[,,] chunkdata;
       
        int chunkSize = owner.chunkSize;

        //if OutOfBounds Block will compare with neighbours in another chunk
        if( x < 0 ||
            y < 0 ||
            z < 0 ||
            x >= chunkSize ||
            y >= chunkSize ||
            z >= chunkSize)
        {

            Vector3 neighChunkPos = owner.goChunk.transform.position + new Vector3(
                (x - (int)origin.x) * chunkSize, 
                (y - (int)origin.y) * chunkSize, 
                (z - (int)origin.z) * chunkSize);

            string chunkName = Chunk.CreateChunkName(neighChunkPos);

            x = ConvertToLocalIndex(x);
            y = ConvertToLocalIndex(y);
            z = ConvertToLocalIndex(z);

            //Gets the neighbour chunk if it cant its air 
            if (WorldOfBlocks.chunkDict.TryGetValue(chunkName, out Chunk neighChunk))
            {
                chunkdata = neighChunk.chunkdata;
            }
            else
            {
                return false;
            }

        }
        else
        {
            chunkdata = owner.chunkdata;
        }

        try
        {
            return chunkdata[x, y, z].isSolid;
        
        }catch (System.IndexOutOfRangeException) { }

        return false;

    }


    public void Draw()
    {
        if (choosenBlockType == BlockType.AIR) return;

        if(!HasSolidNeighbour((int)origin.x -1, (int) origin.y, (int) origin.z))
            CreateBlock(Cubeside.LEFT);
        if (!HasSolidNeighbour((int)origin.x + 1, (int)origin.y, (int)origin.z))
            CreateBlock(Cubeside.RIGHT);
        if (!HasSolidNeighbour((int)origin.x, (int)origin.y-1, (int)origin.z))
            CreateBlock(Cubeside.BOTTOM);
        if (!HasSolidNeighbour((int)origin.x, (int)origin.y+1, (int)origin.z))
            CreateBlock(Cubeside.TOP);
        if (!HasSolidNeighbour((int)origin.x, (int)origin.y, (int)origin.z-1))
            CreateBlock(Cubeside.BACK);
        if (!HasSolidNeighbour((int)origin.x, (int)origin.y, (int)origin.z+1))
            CreateBlock(Cubeside.FRONT);
    }


    void CreateBlock(Cubeside side)
    {
        Vector3[] vertices = new Vector3[4];
        Vector3 v0 = new Vector3(-0.5f, -0.5f, 0.5f);
        Vector3 v1 = new Vector3(0.5f, -0.5f, 0.5f);
        Vector3 v2 = new Vector3(0.5f, -0.5f, -0.5f);
        Vector3 v3 = new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 v4 = new Vector3(-0.5f, 0.5f, 0.5f);
        Vector3 v5 = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 v6 = new Vector3(0.5f, 0.5f, -0.5f);
        Vector3 v7 = new Vector3(-0.5f, 0.5f, -0.5f);

        //int[] triangles = new int[6];
        int[] triangles = new int[] { 3, 1, 0, 3, 2, 1 };

        Vector3[] normals = new Vector3[4];

        switch (side)
        {
            case Cubeside.FRONT:
                vertices = new Vector3[] { v4, v5, v1, v0 };
                normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
                break;

            case Cubeside.BACK:
                vertices = new Vector3[] { v6, v7, v3, v2 };
                normals = new Vector3[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back };
                break;

            case Cubeside.TOP:
                vertices = new Vector3[] { v7, v6, v5, v4 };
                normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
                break;

            case Cubeside.BOTTOM:
                vertices = new Vector3[] { v0, v1, v2, v3 };
                normals = new Vector3[] { Vector3.down, Vector3.down, Vector3.down, Vector3.down };
                break;

            case Cubeside.LEFT:
                vertices = new Vector3[] { v7, v4, v0, v3 };
                normals = new Vector3[] { Vector3.left, Vector3.left, Vector3.left, Vector3.left };
                break;

            case Cubeside.RIGHT:
                vertices = new Vector3[] { v5, v6, v2, v1 };
                normals = new Vector3[] { Vector3.right, Vector3.right, Vector3.right, Vector3.right };
                break;

        }


        Vector2[] autoUV;
        Vector2 currentTextureLBC;

        //Add here in case of multiTextureBlocks
        if (choosenBlockType == BlockType.GRASS)
        {
            if (side == Cubeside.TOP)
            {
                uvBlockByType.TryGetValue((choosenBlockType, CubesideRender.TOP), out currentTextureLBC);
            }
            else if (side == Cubeside.BOTTOM)
            {
                //uvBlockByType.TryGetValue((BlockType.DIRT ,CubesideRender.BOT), out currentTextureLBC);
                uvBlockByType.TryGetValue((choosenBlockType, CubesideRender.BOT), out currentTextureLBC);
            }
            else
            {
                uvBlockByType.TryGetValue((choosenBlockType, CubesideRender.SIDE), out currentTextureLBC);
            }
        }
        else
        {
            uvBlockByType.TryGetValue((choosenBlockType, CubesideRender.ALL), out currentTextureLBC);
        }

        autoUV = GenerateBlockUV(currentTextureLBC);


        Mesh blockMesh = new Mesh
        {
            vertices = vertices,
            normals = normals,
            triangles = triangles,
            //uv = uv
            uv = autoUV
        };

        blockMesh.RecalculateBounds();

        GameObject block = new GameObject("Block");
        block.transform.position = origin;
        block.transform.parent = owner.goChunk.transform;

        MeshFilter blockMeshFilter = block.AddComponent<MeshFilter>();
        blockMeshFilter.mesh = blockMesh;
        MeshRenderer blockMeshRenderer = block.AddComponent<MeshRenderer>();
        blockMeshRenderer.material = material;
    }


    //Vector2[,] blockUVs =
    //{
    //   //GRASS TOP
    //    {
    //        GrassTop_LBC,
    //        GrassTop_LBC + new Vector2(1f,0f)/16,
    //        GrassTop_LBC + new Vector2(0f,1f)/16,
    //        GrassTop_LBC + new Vector2(1f,1f)/16
    //    },
    //    //GRASS SIDE
    //    {
    //        GrassSide_LBC,
    //        GrassSide_LBC + new Vector2(1f,0f)/16,
    //        GrassSide_LBC + new Vector2(0f,1f)/16,
    //        GrassSide_LBC + new Vector2(1f,1f)/16
    //    },
    //    //DORT
    //    {
    //        Dirt_LBC,
    //        Dirt_LBC + new Vector2(1f,0f)/16,
    //        Dirt_LBC + new Vector2(0f,1f)/16,
    //        Dirt_LBC + new Vector2(1f,1f)/16
    //    },
    //    //STONE
    //      {
    //        Stone_LBC,
    //        Stone_LBC + new Vector2(1f,0f)/16,
    //        Stone_LBC + new Vector2(0f,1f)/16,
    //        Stone_LBC + new Vector2(1f,1f)/16
    //    }
    //};


    

    //Vector2 uv00 = new Vector2(0, 0);
    //Vector2 uv01 = new Vector2(0, 1);
    //Vector2 uv10 = new Vector2(1, 0);
    //Vector2 uv11 = new Vector2(1, 1);


    //if (choosenBlockType == BlockType.GRASS && side == Cubeside.TOP)
    //{
    //    uv00 = blockUVs[0, 0];
    //    uv10 = blockUVs[0, 1];
    //    uv01 = blockUVs[0, 2];
    //    uv11 = blockUVs[0, 3];
    //}
    //else if (choosenBlockType == BlockType.GRASS && side == Cubeside.BOTTOM)
    //{
    //    uv00 = blockUVs[2, 0];
    //    uv10 = blockUVs[2, 1];
    //    uv01 = blockUVs[2, 2];
    //    uv11 = blockUVs[2, 3];
    //}
    //else
    //{
    //    uv00 = blockUVs[(int)(choosenBlockType + 1), 0];
    //    uv10 = blockUVs[(int)(choosenBlockType + 1), 1];
    //    uv01 = blockUVs[(int)(choosenBlockType + 1), 2];
    //    uv11 = blockUVs[(int)(choosenBlockType + 1), 3];
    //}
    //Vector2[] uv = new Vector2[] { uv11, uv01, uv00, uv10 };

}
