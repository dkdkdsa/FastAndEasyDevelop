#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FD.AI;

namespace FD.Program.Editer
{

    [CustomEditor(typeof(FAED_TreeAI))]
    public class FAED_TreeEditor : Editor
    {

        public override void OnInspectorGUI()
        {

            base.OnInspectorGUI();

            FAED_TreeAI aI = (FAED_TreeAI)target;

            if (GUILayout.Button("Setting"))
            {

                aI.Setting();

            }

            if (GUILayout.Button("Reset"))
            {

                aI.ResetAI();

            }

        }

    }

}
#endif