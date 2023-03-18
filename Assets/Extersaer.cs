using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extersaer : MonoBehaviour
{

    void Start()
    {



    }

    private void Update()
    {


    }


    IEnumerator ASDF()
    {

        while (true) 
        {

            yield return new WaitForSeconds(1);
            Debug.Log(123123123);

        }



    }

}
