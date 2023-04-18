using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPooling : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {

            FAED.Pop("Test", transform.position, Quaternion.identity);

        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {

        

        }

    }
}
