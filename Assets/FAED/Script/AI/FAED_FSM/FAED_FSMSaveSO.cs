using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FD.AI.FSM
{

    public class FAED_FSMSaveSO : ScriptableObject
    {

        [HideInInspector] public List<FAED_FSMSaveData> nodeData = new List<FAED_FSMSaveData>();
        [HideInInspector] public List<FAEDE_FSMLinkData> linkData = new List<FAEDE_FSMLinkData>();

    }

}