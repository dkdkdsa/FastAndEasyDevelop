using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.AI.FSM
{

    [System.Serializable]
    public class FAED_FSMClass
    {

        public string stateName;
        public GameObject state;
        public List<GameObject> states;

    }


}