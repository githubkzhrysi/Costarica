using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    const int MapLength= 4;

    public List<Vector3> MapPosition = new List<Vector3>
    {
        new Vector3(-4f,0f, 0f),
        new Vector3(-3.5f,0f,-1f),
        new Vector3(-3.0f,0f,-2f),
        new Vector3(-2.5f,0f,-3f),
        new Vector3(-2.0f,0f,-4f),
        new Vector3(-3.5f,0f, 1f),
        new Vector3(-3f,0f, 0f),
        new Vector3(-2.5f,0f,-1f),
        new Vector3(-2.0f,0f,-2f),
        new Vector3(-1.5f,0f,-3f),
        new Vector3(-1f,0f,-4f),
        new Vector3(-3.0f,0f, 2f),
        new Vector3(-2.5f,0f, 1f),
        new Vector3(-2f,0f, 0f),
        new Vector3(-1.5f,0f,-1f),
        new Vector3(-1f,0f,-2f),
        new Vector3(-0.5f,0f,-3f),
        new Vector3(0f,0f,-4f),
        new Vector3(-2.5f,0f, 3f),
        new Vector3(-2.0f,0f, 2f),
        new Vector3(-1.5f,0f, 1f),
        new Vector3(-1f,0f, 0f),
        new Vector3(-0.5f,0f,-1f),
        new Vector3(0f,0f,-2f),
        new Vector3(0.5f,0f,-3f),
        new Vector3(1f,0f,-4f),
        new Vector3(-2f,0f, 4f),
        new Vector3(-1.5f,0f, 3f),
        new Vector3(-1f,0f, 2f),
        new Vector3(-0.5f,0f, 1f),
        new Vector3(0f,0f, 0f),
        new Vector3(0.5f,0f,-1f),
        new Vector3(1f,0f,-2f),
        new Vector3(1.5f,0f,-3f),
        new Vector3(2f,0f,-4f),
        new Vector3(-1f,0f, 4f),
        new Vector3(-0.5f,0f, 3f),
        new Vector3(0f,0f, 2f),
        new Vector3(0.5f,0f, 1f),
        new Vector3(1f,0f, 0f),
        new Vector3(1.5f,0f,-1f),
        new Vector3(2f,0f,-2f),
        new Vector3(2.5f,0f,-3f),
        new Vector3(0f,0f, 4f),
        new Vector3(0.5f,0f, 3f),
        new Vector3(1f,0f, 2f),
        new Vector3(1.5f,0f, 1f),
        new Vector3(2f,0f, 0f),
        new Vector3(2.5f,0f,-1f),
        new Vector3(3f,0f,-2f),
        new Vector3(1f,0f, 4f),
        new Vector3(1.5f,0f, 3f),
        new Vector3(2f,0f, 2f),
        new Vector3(2.5f,0f, 1f),
        new Vector3(3f,0f, 0f),
        new Vector3(3.5f,0f,-1f),
        new Vector3(2f,0f, 4f),
        new Vector3(2.5f,0f, 3f),
        new Vector3(3f,0f, 2f),
        new Vector3(3.5f,0f, 1f),
        new Vector3(4f,0f, 0f),
    };

    public List<GameObject> MapMaterial;

    void Start()
    {
        MapGenerate();
    }      
 
    void MapGenerate()
    {
         for(int i = 0; i < MapPosition.Count; i++)
        {           
            Instantiate(MapMaterial[Random.Range(0, MapMaterial.Count)],MapPosition[i],Quaternion.identity);
        }         
    }
}
    
