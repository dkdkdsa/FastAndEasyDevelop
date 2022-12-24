using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FD.Interfaces;
using FD.Dev;

public class Test2 : MonoBehaviour, IPoolInit
{

    [SerializeField] LayerMask mask;

    private void Awake()
    {


    }
    public void Init()
    {



    }

    void Start()
    {

        //Debug.Log(Mathf.Pow(2, gameObject.layer) == mask);

        

    }

    // Update is called once per frame
    void Update()
    {



    }
}
