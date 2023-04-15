#if UNITY_EDITOR
using FD.UI.Tool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

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

            var saveBtn = toolbar.AddButton(() => { });
            saveBtn.text = "Save";
            var loadBtn = toolbar.AddButton(() => { });
            loadBtn.text = "load";

            var rootNode = new FAED_FSMNode(Guid.NewGuid().ToString(), FAED_FSMNodeType.Root);
            var portBtn = rootNode.titleButtonContainer.AddButton(() =>
            {

                var port = rootNode.AddPort((rootNode.outputContainer.Query("connector").ToList().Count + 1).ToString(),
                                Direction.Output, Port.Capacity.Single);

                var btn = port.AddButton(() =>
                {

                    RemovePort(rootNode, port, port.direction);
                    var lst = rootNode.outputContainer.Query<Port>().ToList();

                    for (int i = 0; i < lst.Count; i++)
                    {

                        lst[i].portName = (i + 1).ToString();
                        lst[i].name = (i + 1).ToString();

                    }

                });
                btn.text = "X";

            });
            portBtn.text = "AddPort";
            CreateNode(rootNode, new Vector2(100, 100), "RootNode", true, false);
            AddNode(rootNode);

            var button = toolbar.AddButton(() =>
            {

                FAED_FSMNode node = new FAED_FSMNode(Guid.NewGuid().ToString(), FAED_FSMNodeType.State);

                CreateNode(node, new Vector2(100, 100), "FSM Node");
                node.AddNameChangeEvent();

                node.AddPort("EnderState", Direction.Input, Port.Capacity.Single);

                 var portBtn =node.titleButtonContainer.AddButton(() =>
                {

                    var port = node.AddPort((node.outputContainer.Query("connector").ToList().Count + 1).ToString(),
                                    Direction.Output, Port.Capacity.Single);

                    var btn = port.AddButton(() =>
                    {

                        RemovePort(node, port, port.direction);
                        var lst = node.outputContainer.Query<Port>().ToList();

                        for(int i = 0; i < lst.Count; i++)
                        {

                            lst[i].portName = (i + 1).ToString();
                            lst[i].name = (i + 1).ToString();

                        }

                    });
                    btn.text = "X"; 

                });

                portBtn.text = "AddPort";

                AddNode(node);

            });
            button.text = "CreateStateNode";

            var trsBtn = toolbar.AddButton(() =>
            {

                var node = new FAED_FSMNode(Guid.NewGuid().ToString(), FAED_FSMNodeType.Transition);
                CreateNode(node, new Vector2(100, 100), "TransitionNode");
                node.AddPort("State", Direction.Input, Port.Capacity.Single);
                AddNode(node);

            });
            trsBtn.text = "CreateTransitionNode";

        }

    }

}
#endif