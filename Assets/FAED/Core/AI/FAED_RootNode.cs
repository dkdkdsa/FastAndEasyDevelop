using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.Dev.AI
{

    public class FAED_RootNode : FAED_Node
    {

        public FAED_Node children;

        protected override FAED_NodeState OnExecute()
        {

            return FAED_NodeState.Success;

        }

    }

}