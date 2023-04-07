#if UNITY_EDITOR
using FD.UI.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FD.AI.FSM
{

    public class FAED_FSMGraphViewWindow : FAED_GraphViewTool<FAED_FSMGraph>
    {

        [MenuItem("FAED_AI/FAED_FSM")]
        private static void CreateWindow()
        {

            var window = GetWindow<FAED_FSMGraphViewWindow>();
            window.titleContent = new GUIContent("FAED_FSM");

        }

        private void OnEnable()
        {

            StartSetting();

        }

        private void OnDisable()
        {

            rootVisualElement.Remove(graphView);
            graphView = null;

        }

        private void StartSetting()
        {

            CreateWindowElement("FAED_FSM", true, true);
            toolbar.AddTextField("FileName");

        }

    }

}
#endif