using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

namespace FD.Core
{

    public class FAED_PoolManager
    {

        private Dictionary<string, Queue<GameObject>> alwaysPoolingContainer = new Dictionary<string, Queue<GameObject>>();
        private Dictionary<string, Queue<GameObject>> scenePoolingContainer = new Dictionary<string, Queue<GameObject>>();
        private FAED_PoolingSO poolingSO;
        private Transform parent;

        private Queue<GameObject> CreatePoolingQueue(int poolCt, string key, GameObject poolObj, Transform parent)
        {

            Queue<GameObject> objQ = new Queue<GameObject>();

            for (int j = 0; j < poolCt; j++)
            {

                var obj = UnityEngine.Object.Instantiate(poolObj, parent);
                obj.gameObject.name = key;
                obj.SetActive(false);
                objQ.Enqueue(obj);

            }

            return objQ;

        }

        public FAED_PoolManager(FAED_PoolingSO poolingSO, Transform parent) 
        { 
            
            this.poolingSO = poolingSO;
            this.parent = parent;

            for(int i = 0; i < poolingSO.alwaysPoolingObjects.Count; i++) 
            { 
                
                var key = poolingSO.alwaysPoolingObjects[i].poolingKey;
                var poolingLS = poolingSO.alwaysPoolingObjects[i];

                Queue<GameObject> objQ = CreatePoolingQueue(poolingLS.poolCount, key, poolingLS.poolingObject, parent);


                if (alwaysPoolingContainer.ContainsKey(key))
                {

                    Debug.LogError($"Please check key duplication : keyName {key}");
                    continue;

                }

                alwaysPoolingContainer.Add(key, objQ);

            }

        }
        public void CreateScenePool(string sceneName)
        {

            Debug.Log(sceneName);

            var pool = poolingSO.scenePoolingObjects.Find(x => x.sceneName == sceneName);

            if (pool == null) return;

            foreach (var item in scenePoolingContainer)
            {

                foreach (var obj in item.Value)
                {

                    UnityEngine.Object.Destroy(obj);

                }

            }

            scenePoolingContainer = new Dictionary<string, Queue<GameObject>>();

            foreach(var obj in pool.scenePoolingObjects)
            {

                var key = obj.poolingKey;
 
                Queue<GameObject> objQ = CreatePoolingQueue(obj.poolCount, key, obj.poolingObject, parent);

                if (scenePoolingContainer.ContainsKey(key))
                {

                    Debug.LogError($"Please check key duplication : keyName {key}");
                    continue;

                }

                scenePoolingContainer.Add(key, objQ);

            }

        }
        public void InsertPool(GameObject obj)
        {

            if(alwaysPoolingContainer.ContainsKey(obj.name)) 
            {

                alwaysPoolingContainer[obj.name].Enqueue(obj);
                obj.transform.SetParent(parent);
                obj.SetActive(false);

            }
            else if (scenePoolingContainer.ContainsKey(obj.name))
            {

                scenePoolingContainer[obj.name].Enqueue(obj);
                obj.transform.SetParent(parent);
                obj.SetActive(false);

            }
            else
            {

                Debug.LogWarning($"Pool named {obj.name} does not exist");
                UnityEngine.Object.Destroy(obj);

            }

        }
        public GameObject TakeOutPool(string key, Nullable<Vector3> pos = null, Nullable<Quaternion> rot = null, Transform parent = null)
        {

            if (pos == null) pos = new Vector3(0, 0, 0);
            if (rot == null) rot = Quaternion.identity;

            if (alwaysPoolingContainer.ContainsKey(key)) 
            {

                var obj = alwaysPoolingContainer[key].Dequeue();
                obj.SetActive(true);
                obj.transform.SetParent(parent);
                obj.transform.position = (Vector3)pos;
                obj.transform.rotation = (Quaternion)rot;

                return obj;

            }
            else if (scenePoolingContainer.ContainsKey(key))
            {

                var obj = alwaysPoolingContainer[key].Dequeue();
                obj.SetActive(true);
                obj.transform.SetParent(parent);
                obj.transform.position = (Vector3)pos;
                obj.transform.rotation = (Quaternion)rot;

                return obj;

            }
            else
            {

                Debug.LogError($"Pool named {key} does not exist");
                return null;
            }

        }
        public T TakeOutPool<T>(string key, Nullable<Vector3> pos = null, Nullable<Quaternion> rot = null, Transform parent = null)
        {

            return TakeOutPool(key, pos, rot, parent).GetComponent<T>();

        }

    }

}