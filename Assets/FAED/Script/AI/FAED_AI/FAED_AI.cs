using FD.AI.Node;
using FD.Program.Editer.SO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.AI
{

    public class FAED_AI : MonoBehaviour
    {

        [SerializeField] private FAED_DialougeContainer data;
        [SerializeField] private FAED_StateMachine machine; 
        
        public void Setting()
        {

            machine = new FAED_StateMachine(data);

        }

        public void ResetAI()
        {

            machine = null;

        }

        private void Update()
        {


            machine.AIActionAll();

        }

    }

}


