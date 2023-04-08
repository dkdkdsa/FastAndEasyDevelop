using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FD.System.Core;
using System;

namespace FD.Dev
{

    public static class FAED
    {

        #region Pooling

        public static GameObject Pop(string poolName, Vector3 pos)
        {

            return FAED_Core.PoolManager.Pop(poolName, pos, Quaternion.identity);

        }

        public static GameObject Pop(string poolName, Vector3 pos, Quaternion rot)
        {

            return FAED_Core.PoolManager.Pop(poolName, pos, rot);

        }

        public static GameObject Pop(string poolName, Vector3 pos, Quaternion rot, Transform parent)
        {

            return FAED_Core.PoolManager.Pop(poolName, pos, rot, parent);

        }

        public static GameObject Pop<T>(string poolName, Vector3 pos, out T component)
        {

            GameObject go = FAED_Core.PoolManager.Pop(poolName, pos, Quaternion.identity);

            component = go.GetComponent<T>();

            return go;

        }

        public static GameObject Pop<T>(string poolName, Vector3 pos, Quaternion rot, out T component)
        {

            GameObject go = FAED_Core.PoolManager.Pop(poolName, pos, rot);

            component = go.GetComponent<T>();

            return go;

        }

        public static GameObject Pop<T>(string poolName, Vector3 pos, Quaternion rot, Transform parent, out T component)
        {

            GameObject go = FAED_Core.PoolManager.Pop(poolName, pos, rot, parent);

            component = go.GetComponent<T>();

            return go;

        }

        public static void Push(GameObject go)
        {

            FAED_Core.PoolManager.Push(go);

        }

        #endregion

        #region Delay

        public static void DelayInvoke(Action action, float delayTime)
        {

            FAED_Core.Delay.InvokeDelay(action, delayTime);

        }

        public static void DelayInvokeRealTime(Action action, float delayTime)
        {

            FAED_Core.Delay.InvokeDelayReal(action, delayTime);

        }

        #endregion

    }

}