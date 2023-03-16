#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FD.UI.Tool;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using System;
using UnityEngine.UIElements;

namespace FD.AI.FSM.Window
{

    public class FAED_FSMEditorWindow : FAED_GraphViewTool
    {

        [MenuItem("FAED_AI/FAED_FSM")]
        private static void ShowWindow()
        {

            var window = CreateWindow<FAED_FSMEditorWindow>();
            window.Show();

        }

        private void OnEnable()
        {

            CreateWindowElement("FAED_FSM", true, true);
            SetToolBar();

        }

        private void SetToolBar()
        {

            #region 파일 이름 입력하는곳

            toolbar.AddTextField("FileName :");
            toolbar.AddButton(() => { }).text = "SaveFile";
            toolbar.AddButton(() => { }).text = "LoadFile";
            toolbar.AddButton(() => CreateNodes()).text = "CreateStateNode";

            #endregion

        }

        private void CreateNodes()
        {

            FAED_FSMNode node = new FAED_FSMNode(Guid.NewGuid().ToString(), "StateNode");

            CreateNode(node,new Vector2(300, 300), node.text);
            node.titleContainer.AddButton(() => 
            {

                int count = node.outputContainer.Query("connector").ToList().Count;
                var port = node.AddPort(count.ToString(), Direction.Output, Port.Capacity.Single);
                port.AddButton(() => node.inputContainer.Remove(port)).text = "X";

            }).text = "+";
            node.AddPort("input", Direction.Input, Port.Capacity.Multi);

            AddNode(node);


        }

    }

    public class FAED_FSMNode : UnityEditor.Experimental.GraphView.Node
    {

        public string GUID;
        public string text;

        public FAED_FSMNode(string GUID, string text) 
        { 
            
            this.GUID = GUID;
            this.text = text;

        }

    }

}

#endif