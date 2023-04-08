using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FD.System.Struct;
using FD.System.SO;

namespace FD.System.Manager
{

    public class FAED_PoolManager
    {
        
        private Dictionary<string, FAED_Pool> container = new Dictionary<string, FAED_Pool>();

        public FAED_PoolManager(FAED_PoolListSO so, Transform parent) 
        { 
        
            foreach (var item in so.poolList)
            {

                container.Add(item.poolName,
                    new FAED_Pool(item.poolCount, item.poolObj, parent, item.poolName));
            
            }

        }

        public GameObject Pop(string poolName, Vector3 pos, Quaternion rot, Transform parent = null)
        {

            return container[poolName].Pop(pos, rot, parent);

        }

    }

}