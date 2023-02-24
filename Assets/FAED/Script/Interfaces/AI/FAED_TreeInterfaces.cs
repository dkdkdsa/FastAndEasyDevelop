using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.AI.Tree.Program
{

    public interface IFAED_TreeAIRoot
    {
        public void Execute();
        public void UpdateEvent();

    }

    public interface IFAED_StateTreeNode<T> where T : IFAED_TreeAIRoot
    {

        public IFAED_TreeAIRoot currentAction { get; set; }
        public List<T> nodeActions { get; set; }
        public int currentNum { get; set; }
        public void CompleteExecution(FAED_TreeNodeState state);

    }

    public interface IFAED_SettingTree<T>
    {

        public T rootNode { get; set; }
        public void SettingRootNode(T rootNode);

    }

}
