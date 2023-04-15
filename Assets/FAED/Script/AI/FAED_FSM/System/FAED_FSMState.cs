using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.AI.FSM
{

    public abstract class FAED_FSMState : MonoBehaviour
    {

        protected List<FAED_FSMTransition> transitions = new List<FAED_FSMTransition>();

        public virtual void SettingState()
        {

            GetComponentsInChildren(transitions);

        }

        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();

    }

}