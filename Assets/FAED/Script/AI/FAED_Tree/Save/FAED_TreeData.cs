using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.AI.Tree
{

    [System.Serializable]
    public class FAED_TreeData : ScriptableObject
    {
    
        public List<FAED_TreeNodeLinkData> links = new List<FAED_TreeNodeLinkData>();
        public List<FAED_TreeNodeData> nodes = new List<FAED_TreeNodeData>();

    }

}