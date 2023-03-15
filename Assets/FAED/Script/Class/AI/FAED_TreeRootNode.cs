using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.AI.Tree.Program
{

    public class FAED_TreeRootNode : FAED_TreeAINode, IFAED_StateTreeNode<FAED_TreeAINode>
    {
        public IFAED_TreeAIRoot currentAction { get; set; }
        public List<FAED_TreeAINode> nodeActions { get; set; } = new List<FAED_TreeAINode>();
        public int currentNum { get; set; }

        //완료 리퀘스트 받음
        public void CompleteExecution(FAED_TreeNodeState state)
        {

            if(state == FAED_TreeNodeState.Failure)
            {

                ExecutionComplete(state);

            }
            else
            {

                Execute();

            }


        }

        //완료 리퀘스트 보냄
        public override void ExecutionComplete(FAED_TreeNodeState state)
        {
            
            if(state == FAED_TreeNodeState.Success)
            {

                Debug.Log("Success");

                currentNum = 0;

            }
            else
            {

                Debug.Log("Failure");

                currentNum = 0;

            }

            ExecuteEvent();

        }

        protected override void ExecuteEvent()
        {

            if(currentNum == nodeActions.Count)
            {

                ExecutionComplete(FAED_TreeNodeState.Success);
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