using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.Dev.AI
{

    public class FAED_RootNode : FAED_Node
    {

        public FAED_Node children;

        public override FAED_NodeState Execute()
        {

            return FAED_NodeState.Success;

        }

    }

}