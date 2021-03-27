using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Script to Generate a world by creating x amount of a choosen block
 */
public class WorldOfCubes : MonoBehaviour
{
    /*
     * Public fields to use on Unity (Inspector)
     */
    //What object is going to be generated
    public GameObject block;
    //Amount of objects (2 = 2x2x2=8)
    public int size;

    void BuildWorld()
    {
        for(int z = 0; z < size; z++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {   
                    Vector3 pos = new Vector3(x, y, z);
                    GameObject cube = GameObject.Instantiate(block, pos, Quaternion.identity);
                }
            }
        }
    }

    /*
     * Coroutine stops on yield and is called each frame till it ends
     */
    IEnumerator BuildProgessivelyWorld()
    {
        for (int z = 0; z < size; z++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Vector3 pos = new Vector3(x, y, z);

                    //Quaternion.identity > No rotation/Default orientation
                    GameObject cube = GameObject.Instantiate(block, pos, Quaternion.identity);

                    cube.name = x + " " + y + " " + z;

                    //each generated cube is child of the object that has the script (Empty - World)
                    cube.transform.parent = this.transform;


                    if(Random.Range(0,100) < 50)
                    {
                        cube.GetComponent<MeshRenderer>().material.color = Color.red;
                    }
                    else
                    {
                        cube.GetComponent<MeshRenderer>().material.color = Color.black;

                    }

                    //yield return null;
                }

                yield return null;
            }

            //yield return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //BuildWorld();
        StartCoroutine(BuildProgessivelyWorld());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
