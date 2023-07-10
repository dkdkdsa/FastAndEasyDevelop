using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.Core
{

    public class FAED_PoolManager
    {

        private Dictionary<string, Queue<GameObject>> poolingContainer = new Dictionary<string, Queue<GameObject>>();

        public FAED_PoolManager(FAED_PoolingSO poolingSO, Transform parent) 
        { 
            
            for(int i = 0; i < poolingSO.alwaysPoolingObjects.Count; i++) 
            { 
                
                var key = poolingSO.alwaysPoolingObjects[i].poolingKey;

                Queue<GameObject> objQ = new Queue<GameObject>();

                for(int j = 0; j < poolingSO.alwaysPoolingObjects[i].poolCount; j++)
                {

                    var obj = Object.Instantiate(poolingSO.alwaysPoolingObjects[i].poolingObject, parent);
                    obj.gameObject.name = key;
                    obj.SetActive(false);
                    objQ.Enqueue(obj);

                }

                if (poolingContainer.ContainsKey(key))
                {

                    Debug.LogError($"Please check key duplication : keyName {key}");
                    continue;

                }

                poolingContainer.Add(key, objQ);

            }

        }

    }

}