using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.AI.Tree.Program
{

    public abstract class FAED_TreeBoolNode : FAED_SettingTree
    {

        public IFAED_TreeAIRoot trueAction;
        public IFAED_TreeAIRoot falseAction;

        public override void Complete(FAED_TreeNodeState state)
        {
            


        }

        public override void Event()
        {

            if (Comparison())
            {

                trueAction.Execute();
                ai.updateEvent.RemoveAllListeners();
                ai.updateEvent.AddListener(trueAction.UpdateEvent);

            }
            else
            {

                falseAction.Execute();
                ai.updateEvent.RemoveAllListeners();
                ai.updateEvent.AddListener(falseAction.UpdateEvent);

            }

        }

        public abstract bool Comparison();

    }

}