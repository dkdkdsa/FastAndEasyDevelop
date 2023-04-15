using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.AI.FSM
{

    [System.Serializable, HideInInspector]
    public class FAED_FSMSaveData
    {

        public string guid;
        public string text;
        public FAED_FSMNodeType type;
        public Vector2 pos;
        public List<string> ports;
        public List<string> inputPorts;

    }

    [System.Serializable, HideInInspector]
    public class FAEDE_FSMLinkData
    {

        public string baseGuid;
        public string portName;
        public string targetGuid;
        public string thisGuid;

    }

}

