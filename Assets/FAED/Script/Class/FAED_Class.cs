using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.Program.Class
{

    [System.Serializable]
    public class FAED_PoolingList
    {

        public string poolName;
        public GameObject poolObj;
        public int poolSize;

    }

    [System.Serializable]
    public class FAED_ClipList
    {

        public string clipName;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0f, 1f)] public float pitch = 1f;
        public bool playOnAwake;
        public bool loop;

    }

}
