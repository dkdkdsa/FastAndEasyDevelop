using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.Dev.AI
{

    public class FAED_SequenceNode : FAED_CompositeNode
    {

        protected override FAED_NodeState OnExecute()
        {
            return FAED_NodeState.Success;
        }

    }

}


