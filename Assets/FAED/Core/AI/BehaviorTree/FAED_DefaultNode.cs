using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.Dev.AI
{

    public class FAED_SequenceNode : FAED_ControlFlowNode
    {

        private int count;

        public override void Init()
        {

            count = 0;

        }

        public override FAED_NodeState Execute()
        {

            var state = childrens[count].Execute();
            count++;

            if (state == FAED_NodeState.Failure) return state;

            if(count != childrens.Count) return FAED_NodeState.Running;

            return FAED_NodeState.Success;

        }

    }

    public class FAED_RootNode : FAED_Node
    {
        public FAED_Node children;
        public override FAED_NodeState Execute()
        {
            throw new System.NotImplementedException();
        }
    }

}