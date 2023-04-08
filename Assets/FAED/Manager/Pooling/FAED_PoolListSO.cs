using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.System.SO
{

    [CreateAssetMenu(fileName = "PoolList", menuName = "FAED/PoolList")]
    public class FAED_PoolListSO : ScriptableObject
    {

        [Serializable]
        public class FAED_PoolList
        {

            public string poolName;
            public int poolCount;
            public GameObject poolObj;

        }

        public List<FAED_PoolList> lists = new List<FAED_PoolList>();

    }

}