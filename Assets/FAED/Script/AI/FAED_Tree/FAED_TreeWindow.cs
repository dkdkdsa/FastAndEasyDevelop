#if  UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FD.UI;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using FD.AI.Tree.Node;

namespace FD.AI.Tree
{

    public class FAED_TreeWindow : FAED_GraphViewWindow<FAED_TreeGrahpView>
    {

        private FAED_TreeGraph treeGraph;
        private string fileName;
        private readonly Vector2 defultNodeSize = new Vector2(x: 150, y: 200);

        [MenuItem("FAED_AI/FAED_Tree")]
        public static void CreateWindow()
        {

            var window = GetWindow<FAED_TreeWindow>();
            window.titleContent = new GUIContent("FAED_Tree");

        }

        private void StartSetting()
        {

            CreateSaveElement();
            graphView.AddNode(CreateStartNode());
            toolbar.Add(CreateSequenceCreateButton());
            toolbar.Add(CreateTreeCreateButton());
            toolbar.Add(CreateIfCreateButton());

        }

        internal FAED_TreeGraphNode CreateStartNode()
        {

            var node = treeGraph.CreateNode("Root", FAED_TreeNodeType.Root, defultNodeSize, true, false, false);

            treeGraph.CreatePortAddButton(node);

            return node;

        }

        private Button CreateSequenceCreateButton()
        {

            var button = new Button(() =>
            {

                var node = treeGraph.CreateNode("Sequence", FAED_TreeNodeType.Sequence, defultNodeSize);

                treeGraph.CreatePort(node, "Input", Direction.Input);
                treeGraph.CreatePortAddButton(node);

                node.inputContainer.Add(new Label("Sequence"));

                graphView.AddNode(node);

            });

            button.text = "Create Sequence";

            return button;

        }

        private Button CreateTreeCreateButton()
        {

            var button = new Button(() =>
            {

                var node = treeGraph.CreateNode("Tree", FAED_TreeNodeType.Tree, defultNodeSize);

                treeGraph.CreatePort(node, "Before", Direction.Input);
                treeGraph.CreatePort(node, "Next", Direction.Output);

                node.inputContainer.Add(new Label("Tree"));

                graphView.AddNode(node);

            });

            button.text = "Create Tree";

            return button;

        }

        private Button CreateIfCreateButton()
        {

            var button = new Button(() =>
            {

                var node = treeGraph.CreateNode("If", FAED_TreeNodeType.If, defultNodeSize);

                treeGraph.CreatePort(node, "Before", Direction.Input);
                treeGraph.CreatePort(node, "NextTrue", Direction.Output);
                treeGraph.CreatePort(node, "NextFalse", Direction.Output);

                node.inputContainer.Add(new Label("If"));

                graphView.AddNode(node);

            });

            button.text = "Create If";

            return button;

        }

        private void CreateSaveElement()
        {

            var fileNameTextField = new TextField(label: "FileName");
            fileNameTextField.SetValueWithoutNotify(fileName);
            fileNameTextField.MarkDirtyRepaint();
            fileNameTextField.RegisterValueChangedCallback(evt => fileName = evt.newValue);
            toolbar.Add(fileNameTextField);

            toolbar.Add(new Button(() => RequestDataOperation(true)) { text = "SaveData" });
            toolbar.Add(new Button(() => RequestDataOperation(false)) { text = "LoadData" });

        }

        private void RequestDataOperation(bool value)
        {

            if (string.IsNullOrEmpty(fileName))
            {

                EditorUtility.DisplayDialog("error", "Invalid File Name", "Ok");
                return;

            }

            var grahpSave = FAED_TreeSave.GetInstance(treeGraph, graphView, this);

            if (value)
            {

                grahpSave.SaveGraph(fileName);

            }
            else
            {

                grahpSave.LoadGraph(fileName);

            }

        }

        private void OnEnable()
        {
            
            CreateWindowElement("FAED_Tree", true, true);
            treeGraph = new FAED_TreeGraph(graphView);
            StartSetting();

        }

        private void OnDisable()
        {

            rootVisualElement.Remove(graphView);
            treeGraph = null;

        }

    }

    public class FAED_TreeGrahpView : FAED_GraphView
    {

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {

            var compatiblePorts = new List<Port>();

            ports.ForEach(funcCall: (port) =>
            {

                if (startPort != port && startPort.node != port.node)
                {

                    var start = startPort.node.Q<FAED_TreeGraphNode>().type;
                    var end = port.node.Q<FAED_TreeGraphNode>().type;

                    if (!((start == FAED_TreeNodeType.Tree || start == FAED_TreeNodeType.If) && end == FAED_TreeNodeType.Sequence))
                    {

                        compatiblePorts.Add(port);

                    }


                }

            });

            return compatiblePorts;

        }


    }

}
#endif