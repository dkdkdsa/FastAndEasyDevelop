#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEditor;
using UnityEngine;

namespace FD.AI.FSM
{

    [CustomEditor(typeof(FAED_FSM))]
    public class FAED_FSMUI : Editor
    {

        public override void OnInspectorGUI()
        {

            base.OnInspectorGUI();

            var fsm = (FAED_FSM)target;

            if (GUILayout.Button("Setting")) fsm.Setting();

        }

    }

}
#endif