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

            return children.Execute();

        }

        public override FAED_Node Copy(FAED_BehaviorTreeRunner runner)
        {

            this.runner = runner;

            var node = Instantiate(this);
            node.children = children.Copy(runner);

            return node;

        }

    }

}