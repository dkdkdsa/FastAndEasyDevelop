using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FD.Core;

namespace FD.Dev
{

    public static class FAED
    {

        public static void InsertPool(GameObject obj)
        {

            FAED_Core.PoolManager.InsertPool(obj);

        }

        public static GameObject TakePool(string poolName)
        {

            return FAED_Core.PoolManager.TakePool(poolName);

        }
        public static T TakePool<T>(string poolName)
        {

            return FAED_Core.PoolManager.TakePool<T>(poolName);

        }
        public static GameObject TakePool(string poolName, Vector3 pos)
        {

            return FAED_Core.PoolManager.TakePool(poolName, pos);

        }
        public static T TakePool<T>(string poolName, Vector3 pos)
        {

            return FAED_Core.PoolManager.TakePool<T>(poolName, pos);

        }
        public static GameObject TakePool(string poolName, Vector3 pos, Quaternion rot) 
        {
            
            return FAED_Core.PoolManager.TakePool(poolName, pos, rot);

        }
        public static T TakePool<T>(string poolName, Vector3 pos, Quaternion rot)
        {

            return FAED_Core.PoolManager.TakePool<T>(poolName, pos, rot);

        }
        public static GameObject TakePool(string poolName, Vector3 pos, Quaternion rot, Transform parent)
        {

            return FAED_Core.PoolManager.TakePool(poolName, pos, rot, parent);

        }
        public static T TakePool<T>(string poolName, Vector3 pos, Quaternion rot, Transform parent)
        {

            return FAED_Core.PoolManager.TakePool<T>(poolName, pos, rot, parent);

        }

    }

}