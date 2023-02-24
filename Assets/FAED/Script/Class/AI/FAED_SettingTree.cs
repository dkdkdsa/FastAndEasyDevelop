using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.AI.Tree.Program
{

    public abstract class FAED_SettingTree : FAED_TreeAINode, IFAED_SettingTree<IFAED_StateTreeNode<FAED_TreeAINode>>
    {
        public IFAED_StateTreeNode<FAED_TreeAINode> rootNode { get; set; }

        //º¸³¿
        public override void ExecutionComplete(FAED_TreeNodeState state)
        {

            Complete(state);

        }

        public void SettingRootNode(IFAED_StateTreeNode<FAED_TreeAINode> rootNode)
        {

            this.rootNode = rootNode;

        }

        protected override void ExecuteEvent()
        {

            Event();

        }
        
        public abstract void Complete(FAED_TreeNodeState state);
        public abstract void Event();

    }


}

