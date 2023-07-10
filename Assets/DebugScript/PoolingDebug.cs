using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FD.Dev;

public class PoolingDebug : MonoBehaviour
{

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.H))
        {

            var obj = FAED.TakePool("A");
            FAED.InsertPool(obj);

        }

    }

}
