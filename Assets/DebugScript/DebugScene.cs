using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugScene : MonoBehaviour
{


    private void Awake()
    {

        Vector3 v = Vector3.one;
        v = Vector2.one;

        Debug.Log(v);

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {

            SceneManager.LoadScene("TestA");

        }

        if (Input.GetKeyDown(KeyCode.B))
        {

            SceneManager.LoadScene("TestB");

        }

    }

}
