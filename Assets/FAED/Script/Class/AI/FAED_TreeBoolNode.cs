using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.AI.Tree.Program
{

    public abstract class FAED_TreeBoolNode : FAED_SettingTree, IFAED_StateTreeNode<FAED_TreeAINode>
    {

        [HideInInspector] public List<FAED_TreeAINode> trueAction;
        [HideInInspector] public List<FAED_TreeAINode> falseAction;

        public IFAED_TreeAIRoot currentAction { get; set; }
        public List<FAED_TreeAINode> nodeActions { get; set; }
        public int currentNum { get; set; }

        public override void Event()
        {

            if (Comparison())
            {

                nodeActions = trueAction;

            }
            else
            {
                
                nodeActions = falseAction;

            }

            BoolEx();

        }

        public abstract bool Comparison();

        //¿Ï·á ¸®Äù½ºÆ® º¸³»´Â ³à¼®
        public override void Complete(FAED_TreeNodeState state)
        {

            rootNode.CompleteExecution(state);

            currentNum = 0;

        }


        //¿Ï·á ¸®Äù½ºÆ® ¹Þ´Â ³à¼®
        public void CompleteExecution(FAED_TreeNodeState state)
        {


            if (state == FAED_TreeNodeState.Success)
            {

                BoolEx();

            }
            else
            {

                Complete(state);

            }

        }

        public void BoolEx()
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