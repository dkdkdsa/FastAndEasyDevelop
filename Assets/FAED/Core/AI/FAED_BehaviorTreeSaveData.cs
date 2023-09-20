using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.Dev.AI
{
    public class FAED_BehaviorTreeSaveData : ScriptableObject
    {

        public FAED_BehaviorTree behaviorTree;
        public List<FAED_ConnectData> connectData = new List<FAED_ConnectData>();

    }

}

