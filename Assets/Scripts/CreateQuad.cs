using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateQuad : MonoBehaviour
{
    enum Cubeside { FRONT, BACK, TOP, BOTTOM, LEFT,RIGHT}


    public Material material;
    
    public bool isFrontRendered = true;
    public bool isBackRendered = true;
    public bool isTopRendered = true;
    public bool isBotRendered = true;
    public bool isLeftRendered = true;
    public bool isRightRendered = true;

    public bool isTriangularFaces = false;

    bool isQuadNotCube = false;
    int[] sides;

    /*
     * Template function it's just a demonstration of the Cube function
     */
    void Quad()
    {
        /*
          * Cube with 8 vertices center 0,0,0
          */
        //front bottom right
        Vector3 v0 = new Vector3(-0.5f, -0.5f,  0.5f);
        //front bottom left
        Vector3 v1 = new Vector3( 0.5f, -0.5f,  0.5f);
        //back bottom left
        Vector3 v2 = new Vector3( 0.5f, -0.5f, -0.5f);
        //back bottom right
        Vector3 v3 = new Vector3(-0.5f, -0.5f, -0.5f);
        //front top right
        Vector3 v4 = new Vector3(-0.5f,  0.5f,  0.5f);
        //front top left
        Vector3 v5 = new Vector3( 0.5f,  0.5f,  0.5f);
        //back top left
        Vector3 v6 = new Vector3( 0.5f,  0.5f, -0.5f);
        //back top right
        Vector3 v7 = new Vector3(-0.5f,  0.5f, -0.5f);

        //current quad vertices to make the cube face
        Vector3[] vertices = new Vector3[] { v4, v5, v1, v0 };


        //one cube face has 2 triangles made by 3 vertices clockwise
        //  ex: front face - v5(1),v4(0),v0(3),v1(2) triangles 310 (v0,v5,v4) and 321 (v0,v1,v5)
        int[] triangles = new int[] { 3, 1, 0, 3, 2, 1 };


        //perpendicular vector with cube face
        Vector3 n = new Vector3(0, 0, 1);
        // ex:front face - 0,0,1 -> forward vector; top face - 0,1,0 -> up vector
        Vector3[] normals = new Vector3[] { n, n, n, n };


        //orthonomatic axis to render the quad (map image or lightmaps) each vector maps a vertice
        Vector2 uv00 = new Vector2(0, 0);
        Vector2 uv01 = new Vector2(0, 1);
        Vector2 uv10 = new Vector2(1, 0);
        Vector2 uv11 = new Vector2(1, 1);

        //so to make an upside down image map the inverse y axis
        Vector2[] uv = new Vector2[] { uv11, uv01, uv00, uv10 };

        /*
         * Change the generated gameObject based on this script
         */
        Mesh myMesh = new Mesh();
        myMesh.vertices = vertices;
        myMesh.normals = normals;
        myMesh.triangles = triangles;
        myMesh.uv = uv;

        MeshFilter myMeshFilter = this.gameObject.AddComponent<MeshFilter>();
        myMeshFilter.mesh = myMesh;
        MeshRenderer myMeshRenderer = this.gameObject.AddComponent<MeshRenderer>();
        myMeshRenderer.material = material;
    }

    /*
     * Create quads based on the choosen sides
     */ 
    void oldCube(int side)
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


        Vector2[] uv = new Vector2[4];
        Vector2 uv00 = new Vector2(0, 0);
        Vector2 uv01 = new Vector2(0, 1);
        Vector2 uv10 = new Vector2(1, 0);
        Vector2 uv11 = new Vector2(1, 1);
        uv = new Vector2[] { uv11, uv01, uv00, uv10 };


        Vector3[] normals = new Vector3[4];
       

        int[] triangles = new int[6];
        triangles = new int[] { 3, 1, 0, 3, 2, 1 };

        /*
         * The sequence of vertices dont matter but clockwise renders outside and counter clock inside
         * Although the order of the vertices change how its rendered
         */
        switch (side)
        {
            case (int)Cubeside.FRONT:
                //vertices = new Vector3[] { v4, v1, v6, v0 };
                vertices = new Vector3[] { v5, v1, v0, v4 };
                vertices = new Vector3[] { v4, v5, v1, v0 };
                normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
                break;

            case (int)Cubeside.BACK:
                vertices = new Vector3[] { v6, v7, v3, v2 };
                normals = new Vector3[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back };
                break;

            case (int)Cubeside.TOP:
                vertices = new Vector3[] { v7, v6, v5, v4 };
                normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
                break;

            case (int)Cubeside.BOTTOM:
                vertices = new Vector3[] { v0, v1, v2, v3 };
                normals = new Vector3[] { Vector3.down, Vector3.down, Vector3.down, Vector3.down };
                break;

            case (int)Cubeside.LEFT:
                vertices = new Vector3[] { v7, v4, v0, v3 };
                normals = new Vector3[] { Vector3.left, Vector3.left, Vector3.left, Vector3.left };
                break;

            case (int)Cubeside.RIGHT:
                vertices = new Vector3[] { v5, v6, v2, v1 };
                normals = new Vector3[] { Vector3.right, Vector3.right, Vector3.right, Vector3.right };
                break;

        }


        GameObject quad = new GameObject("quad");
        quad.transform.parent = quad.transform;


        Mesh myMesh = new Mesh();
        myMesh.vertices = vertices;
        myMesh.normals = normals;
        myMesh.triangles = triangles;
        myMesh.uv = uv; 

        //MeshFilter myMeshFilter = this.gameObject.AddComponent<MeshFilter>();
        MeshFilter myMeshFilter = quad.AddComponent<MeshFilter>();
        myMeshFilter.mesh = myMesh;
        //MeshRenderer myMeshRenderer = this.gameObject.AddComponent<MeshRenderer>();
        MeshRenderer myMeshRenderer = quad.AddComponent<MeshRenderer>();
        myMeshRenderer.material = material;

    }

    void CreateCube()
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

        Vector3[] normals = new Vector3[4];


        if (isFrontRendered)
        {
            vertices = new Vector3[] { v4, v5, v1, v0 };
            normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
            InstantiateCube(vertices,normals);
        }
        if (isBackRendered)
        {
            vertices = new Vector3[] { v6, v7, v3, v2 };
            normals = new Vector3[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back };
            InstantiateCube(vertices, normals);
        }
        if (isTopRendered)
        {
            vertices = new Vector3[] { v7, v6, v5, v4 };
            normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
            InstantiateCube(vertices, normals);

        }
        if (isBotRendered)
        {
            vertices = new Vector3[] { v0, v1, v2, v3 };
            normals = new Vector3[] { Vector3.down, Vector3.down, Vector3.down, Vector3.down };
            InstantiateCube(vertices, normals);

        }
        if (isLeftRendered)
        {
            vertices = new Vector3[] { v7, v4, v0, v3 };
            normals = new Vector3[] { Vector3.left, Vector3.left, Vector3.left, Vector3.left };
            InstantiateCube(vertices, normals);

        }
        if (isRightRendered)
        {
            vertices = new Vector3[] { v5, v6, v2, v1 };
            normals = new Vector3[] { Vector3.right, Vector3.right, Vector3.right, Vector3.right };
            InstantiateCube(vertices, normals);
        }

       

        CombineQuads();
    }

    private void InstantiateCube(Vector3[] vertices, Vector3[] normals)
    {
        GameObject quad = new GameObject("childQuad");
        quad.transform.parent = this.gameObject.transform;

        Vector2[] uv = new Vector2[4];
        Vector2 uv00 = new Vector2(0, 0);
        Vector2 uv01 = new Vector2(0, 1);
        Vector2 uv10 = new Vector2(1, 0);
        Vector2 uv11 = new Vector2(1, 1);
        uv = new Vector2[] { uv11, uv01, uv00, uv10 };

        int[] triangles = new int[6];
        triangles = new int[] { 3, 1, 0, 3, 2, 1 };

        if (isTriangularFaces)
        {
            triangles = new int[] { 3, 1, 0, 3, 1, 2 };
        }



        Mesh myMesh = new Mesh();
        myMesh.vertices = vertices;
        myMesh.normals = normals;
        myMesh.triangles = triangles;
        myMesh.uv = uv;

        //MeshFilter myMeshFilter = this.gameObject.AddComponent<MeshFilter>();
        MeshFilter myMeshFilter = quad.AddComponent<MeshFilter>();
        myMeshFilter.mesh = myMesh;
    }

    void CombineQuads()
    {
        //1.Combine all children meshes
        MeshFilter[] childMeshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combinedChildMeshes = new CombineInstance[childMeshFilters.Length];

        int i = 0;
        while(i < childMeshFilters.Length)
        {
            combinedChildMeshes[i].mesh = childMeshFilters[i].sharedMesh;
            combinedChildMeshes[i].transform = childMeshFilters[i].transform.localToWorldMatrix;
            i++;
        }

        //2. Create a new mesh on the parent object
        //MeshFilter mf = (MeshFilter)this.gameObject.AddComponent(typeof(MeshFilter));
        MeshFilter parentMeshFilter = this.gameObject.AddComponent<MeshFilter>();

        //3. Add combined meshes of children as the parent's mesh
        parentMeshFilter.mesh.CombineMeshes(combinedChildMeshes);

        //4. Create a renderer for the parent
        MeshRenderer parentRenderer = this.gameObject.AddComponent<MeshRenderer>();
        parentRenderer.material = material;

        //5. Delete all uncombined children
        foreach(Transform quad in this.transform)
        {
            Destroy(quad.gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        //oldStart();
        CreateCube();

    }

    private void oldStart()
    {
        if (isQuadNotCube)
        {
            Quad();
        }
        else
        {
            /*
             * Verifications
             */
            if (sides.Length <= 0)
            {
                throw new System.Exception("Need one side of the cube");
            }

            if (sides.Length > 6)
            {
                throw new System.Exception("Cube only has 6 faces");
            }

            int j = 0;
            for (int i = 1; i < sides.Length; i++)
            {

                if (sides[j] < 0 || sides[j] > 5)
                {
                    throw new System.Exception("Faces are 0 to 5");
                }

                if (sides[i] == sides[j])
                {
                    Debug.Log("i:" + sides[i]);
                    Debug.Log("j:" + sides[j]);
                    throw new System.Exception("Choosen sides shouldn't repeat");
                }

                j++;
            }

            for (int i = 0; i < sides.Length; i++)
            {
                oldCube(i);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
