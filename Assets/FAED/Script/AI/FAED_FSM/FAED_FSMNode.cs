#if UNITY_EDITOR
using FD.AI.Nodes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace FD.AI.FSM
{

    public class FAED_FSMNode : Node
    {

        public string text;
        public string GUID;
        public FAED_FSMNodeType nodeType;

        public FAED_FSMNode(string text, string GUID, FAED_FSMNodeType nodeType)
        {

            this.text = text;
            this.GUID = GUID;
            this.nodeType = nodeType;

        }

    }

}
#endif