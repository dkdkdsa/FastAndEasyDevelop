using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FD.Interfaces;
using FD.Dev;

public class Test2 : MonoBehaviour
{


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            FAED.StopSound("Test");

        }

        if (Input.GetKeyDown(KeyCode.Return))
        {

            FAED.PlaySound("Test2");

        }

    }
}
