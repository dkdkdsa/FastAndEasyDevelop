using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugScene : MonoBehaviour
{

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
