using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FD.Dev.CSG;

public class Sub : MonoBehaviour
{

    [SerializeField] private GameObject a, b;


    private void Awake()
    {

        var res = FAED_CSG.Subtract(a, b);

        var obj = new GameObject();
        obj.AddComponent<MeshFilter>().sharedMesh = res;
        obj.AddComponent<MeshCollider>().sharedMesh = res;
        obj.AddComponent<MeshRenderer>();

    }

}
