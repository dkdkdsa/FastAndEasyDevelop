using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.System.Class
{
    public struct FAED_Pool
    {

        private Queue<GameObject> poolContainer;
        private GameObject poolObj;
        
        public FAED_Pool(int poolCount,  GameObject poolObj)
        {

            this.poolObj = poolObj;
            poolContainer = new Queue<GameObject>();

            for(int i = 0; i < poolCount; i++)
            {

                poolContainer.Enqueue();

            }

        }

    }

}