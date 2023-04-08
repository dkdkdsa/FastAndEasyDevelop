using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.System.Struct
{
    public struct FAED_Pool
    {

        private Queue<GameObject> poolContainer;
        private GameObject poolObj;
        private Transform parent;
        private int poolCount;

        public string poolName { get; private set; }
        
        public FAED_Pool(int poolCount,  GameObject poolObj, Transform parent, string poolName)
        {

            this.poolObj = poolObj;
            this.parent = parent;
            this.poolName = poolName;
            this.poolCount = poolCount;
            poolContainer = new Queue<GameObject>();

            for(int i = 0; i < poolCount; i++)
            {

                GameObject obj = Object.Instantiate(poolObj);
                obj.transform.parent = parent;
                obj.gameObject.SetActive(false);
                obj.name = poolName;

                poolContainer.Enqueue(obj);

            }

        }

        public GameObject Pop(Vector3 pos, Quaternion rot, Transform parent = null)
        {

            GameObject go;

            if (poolContainer.Count < 0)
            {

                go = Object.Instantiate(poolObj);
                go.gameObject.name = poolName;
                go.transform.SetParent(parent);
                go.transform.SetPositionAndRotation(pos, rot);
                Debug.LogWarning($" FAED_PoolManager : " +
                    $"The number of uses of the {poolName} pool has exceeded the pool count. " +
                    $"It is recommended to increase the pool count (current pool count: {poolCount})");
                return go;

            }

            go = poolContainer.Dequeue();
            go.SetActive(true);
            go.transform.SetParent(parent);
            go.transform.SetPositionAndRotation(pos, rot);
            return go;


        }

        public void Push(GameObject obj)
        {

            obj.SetActive(false);
            obj.transform.SetParent(parent);
            poolContainer.Enqueue(obj);

        }

    }

}