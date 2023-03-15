using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.AI.Tree.Program
{

    public abstract class FAED_TreeAINode : MonoBehaviour, IFAED_TreeAIRoot
    {

        private string GUID;
        private bool excuteAble;

        protected FAED_TreeAI ai;
        protected FAED_TreeAIMain main;
        
        protected GameObject mainGameObject => ai.gameObject;
        protected Transform mainTransform => ai.transform;

        protected abstract void ExecuteEvent();

        public void Execute()
        {

            excuteAble = true;

        }

        public abstract void ExecutionComplete(FAED_TreeNodeState state);

        public void Setting(FAED_TreeAIMain main, FAED_TreeAI aI, string GUID)
        {

            ai = aI;
            this.main = main;
            this.GUID = GUID;

        }

        public virtual void UpdateEvent() { }
        
        private void Update()
        {

            if (excuteAble)
            {

                ExecuteEvent();
                excuteAble = false;

            }

        }

    }

}