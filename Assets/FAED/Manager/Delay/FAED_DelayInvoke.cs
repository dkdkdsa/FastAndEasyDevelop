using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.System.Delay
{

    public class FAED_DelayInvoke : MonoBehaviour
    {

        public void InvokeDelay(Action action, float delSec)
        {

            StartCoroutine(DelayCo(action, delSec));

        }

        public void InvokeDelayReal(Action action, float delSec)
        {

            StartCoroutine(RealDelayCo(action, delSec));

        }

        IEnumerator DelayCo(Action action, float delSec)
        {

            yield return new WaitForSeconds(delSec);
            action?.Invoke();

        }

        IEnumerator RealDelayCo(Action action, float delSec)
        {

            yield return new WaitForSecondsRealtime(delSec);
            action?.Invoke();

        }

    }

}