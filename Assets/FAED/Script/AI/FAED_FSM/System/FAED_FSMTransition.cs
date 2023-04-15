using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.AI.FSM
{

    public abstract class FAED_FSMTransition : MonoBehaviour
    {

        [HideInInspector] public string nextState;

        public void SettingTransition(string nextState)
        {

            this.nextState = nextState;

        }

        public abstract bool ChackTransition();

    }

}