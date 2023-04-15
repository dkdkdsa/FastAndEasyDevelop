#if UNITY_EDITOR
using FD.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FD.AI.FSM
{
    public class FAED_FSMSave
    {

        private FAED_GraphView view;

        public FAED_FSMSave(FAED_GraphView view)
        {

            this.view = view;

        }

        public void Save()
        {

            view.Query<FAED_FSMNode>().ToList().ForEach(x =>
            {

                

            });

        }

    }

}
#endif