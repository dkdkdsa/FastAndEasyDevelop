using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.AI.Tree.Program
{

    public class FAED_Sequence : FAED_SettingTree, IFAED_StateTreeNode<FAED_TreeAINode>
    {
        public IFAED_TreeAIRoot currentAction { get; set; }
        public List<FAED_TreeAINode> nodeActions { get; set; } = new List<FAED_TreeAINode>();
        public int currentNum { get; set; }


        //완료 리퀘스트 보내는 녀석
        public override void Complete(FAED_TreeNodeState state)
        {

            rootNode.CompleteExecution(state);

            currentNum = 0;

        }


        //완료 리퀘스트 받는 녀석
        public void CompleteExecution(FAED_TreeNodeState state)
        {


            if (state == FAED_TreeNodeState.Success)
            {

                Event();

            }
            else
            {

                Complete(state);

            }

        }

        public override void Event()
        {

            if (currentNum == nodeActions.Count)
            {

                Complete(FAED_TreeNodeState.Success);
                return;

            }

            currentAction = nodeActions[currentNum];

            ai.updateEvent.RemoveAllListeners();

            ai.updateEvent.AddListener(currentAction.UpdateEvent);

            currentNum++;

            currentAction.Execute();

        }

    }

}

