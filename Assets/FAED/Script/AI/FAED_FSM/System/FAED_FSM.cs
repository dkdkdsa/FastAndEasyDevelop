using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.AI.FSM
{

    public class FAED_FSM : MonoBehaviour
    {

        [SerializeField, HideInInspector] private List<FAED_FSMClass> fsmList = new List<FAED_FSMClass>();
        [SerializeField] private FAED_FSMSaveSO data;
        [SerializeField] private FAED_FSMState firstState; 

        private List<FAED_FSMTransition> transitions = new List<FAED_FSMTransition>(); 

        public event Action EnterEvnet;
        public event Action UpdateEvnet;
        public event Action ExitEvnet;


        private void Awake()
        {
            
            if(firstState == null) 
            { 
                
                firstState = transform.GetChild(0).GetComponent<FAED_FSMState>();

            }



        }

        public void Setting()
        {

            if (data == null) return;


            var nodes = data.nodeData.FindAll(x => x.type == FAED_FSMNodeType.State);
            foreach ( var node in nodes ) 
            {

                GameObject obj = new GameObject(node.text + "(State)");

                var trs = data.linkData.FindAll(x => x.baseGuid == node.guid);

                var trsObj = new List<GameObject>();

                trs.ForEach(x =>
                {

                    var text = data.nodeData.Find(y => y.guid == x.targetGuid).text;
                    var trsOb = new GameObject("GoTo" + text);
                    trsOb.transform.SetParent(obj.transform);
                    trsObj.Add(trsOb);

                });

                fsmList.Add(new FAED_FSMClass
                {

                    stateName = node.text,
                    state = obj,
                    states = trsObj

                });

            }

        }

        private void Update()
        {

            UpdateEvnet();
            TryChangeState();

        }

        private void TryChangeState()
        {

            foreach(var trs in transitions)
            {

                if(trs.ChackTransition() == true)
                {

                    ExitEvnet?.Invoke();
                    ExitEvnet = null;
                    ChangeState(trs.nextState);

                }

            }

        }

        private void ChangeState(string nextState)
        {

            var next = fsmList.Find(x => x.stateName == nextState);

            var state = next.state.GetComponent<FAED_FSMState>();

            EnterEvnet += state.EnterState;
            UpdateEvnet += state.UpdateState;
            ExitEvnet += state.ExitState;

            transitions = null;

            foreach(var trs in next.states)
            {
                
                transitions.Add(trs.GetComponent<FAED_FSMTransition>());

            }

            EnterEvnet?.Invoke();

        }

    }

}