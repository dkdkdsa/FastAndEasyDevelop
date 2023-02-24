using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.AI.Tree.Program
{
    public abstract class FAED_TreeActionNode : FAED_SettingTree
    {
        public override void Complete(FAED_TreeNodeState state)
        {

            rootNode.CompleteExecution(state);

        }

        public override void Event()
        {

            Behavior();

        }

        public abstract void Behavior();

    }

}