using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.Dev.AI
{

    public class FAED_DebugNode : FAED_ActionNode
    {

        [SerializeField] private string debugText;

        protected override FAED_NodeState OnExecute()
        {

            Debug.Log(debugText);

            return FAED_NodeState.Failure;

        }
    }

}