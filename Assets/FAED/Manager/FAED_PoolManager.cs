using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.Core
{

    public class FAED_PoolManager
    {

        private Dictionary<string, Queue<GameObject>> alwaysPoolingContainer = new Dictionary<string, Queue<GameObject>>();
        private Dictionary<string, Queue<GameObject>> scenePoolingContainer = new Dictionary<string, Queue<GameObject>>();
        private FAED_PoolingSO poolingSO;
        private Transform parent;

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

                    Object.Destroy(obj);

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


        private Queue<GameObject> CreatePoolingQueue(int poolCt, string key, GameObject poolObj, Transform parent)
        {

            Queue<GameObject> objQ = new Queue<GameObject>();

            for (int j = 0; j < poolCt; j++)
            {

                var obj = Object.Instantiate(poolObj, parent);
                obj.gameObject.name = key;
                obj.SetActive(false);
                objQ.Enqueue(obj);

            }

            return objQ;

        }

    }

}