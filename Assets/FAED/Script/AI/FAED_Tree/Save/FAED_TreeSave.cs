#if UNITY_EDITOR
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
using FD.AI.Tree.Nodes;

namespace FD.AI.Tree
{
    public class FAED_TreeSave
    {

        private FAED_TreeGraph treeGraph;
        private FAED_GraphView graphView;
        private FAED_TreeWindow treeWindow;
        private FAED_TreeData data;
        private readonly Vector2 defultNodeSize = new Vector2(x: 150, y: 200);

        private List<Edge> edges => graphView.edges.ToList();
        private List<FAED_TreeGraphNode> nodes => graphView.nodes.ToList().Cast<FAED_TreeGraphNode>().ToList();

        internal static FAED_TreeSave GetInstance(FAED_TreeGraph treeGraph, FAED_GraphView graphView, FAED_TreeWindow treeWindow)
        {

            return new FAED_TreeSave
            {

                treeGraph = treeGraph,
                graphView = graphView,
                treeWindow = treeWindow

            };
            
        }

        internal void SaveGraph(string fileName)
        {

            if (!edges.Any()) return;

            var data = ScriptableObject.CreateInstance<FAED_TreeData>();
            var connectedPorts = edges.Where(x => x.input.node != null).ToList();

            foreach(var item in connectedPorts)
            {

                var output = item.output.node as FAED_TreeGraphNode;
                var input = item.input.node as FAED_TreeGraphNode;

                data.links.Add(new FAED_TreeNodeLinkData
                {

                    baseGUID = output.GUID,
                    portName = item.output.portName,
                    targetGUID = input.GUID,

                });

            }

            foreach(var dataNode in nodes)
            {

                data.nodes.Add(new FAED_TreeNodeData
                {

                    GUID = dataNode.GUID,
                    text = dataNode.text,
                    pos = dataNode.GetPosition().position,
                    type = dataNode.type,
                    ports = dataNode.outputContainer.Query<Port>().ToList().Select(x => x.portName).ToList(),
                    inputPorts = dataNode.inputContainer.Query<Port>().ToList().Select(x => x.portName).ToList()

                });

            }

            AssetDatabase.CreateAsset(data, $"Assets/Resources/FAED/Tree/{fileName}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

        }

        internal void LoadGraph(string fileName)
        {

            data = Resources.Load<FAED_TreeData>($"FAED/Tree/{fileName}");

            if (data == null)
            {

                EditorUtility.DisplayDialog("error", "File Not Found", "ok");
                return;

            }

            ClearGrahp();
            CreateNodes();
            ConnectModes();

        }
        private void ClearGrahp()
        {

            foreach (var node in nodes)
            {

                edges.Where(x => x.input.node == node).ToList().ForEach(edge => graphView.RemoveElement(edge));

                graphView.RemoveElement(node);

            }

        }
        
        private void CreateNodes()
        {

            foreach(var nodeData in data.nodes)
            {

                if (nodeData.type == FAED_TreeNodeType.Root)
                {

                    var node = treeWindow.CreateStartNode();
                    node.GUID = nodeData.GUID;

                    foreach (var item in nodeData.ports)
                    {

                        treeGraph.CreateButtonPort(node, item);

                    }

                    graphView.AddNode(node);

                    node.SetPosition(new Rect(nodeData.pos, defultNodeSize));

                }

                if(nodeData.type == FAED_TreeNodeType.Sequence)
                {

                    var node = treeGraph.CreateNode(nodeData.text, FAED_TreeNodeType.Sequence, defultNodeSize);
                    node.GUID = nodeData.GUID;

                    treeGraph.CreatePort(node, "Input", Direction.Input);
                    treeGraph.CreatePortAddButton(node);
                    

                    foreach(var item in nodeData.ports)
                    {

                        treeGraph.CreateButtonPort(node, item);

                    }

                    node.inputContainer.Add(new Label("Sequence"));


                    graphView.AddNode(node);

                    node.SetPosition(new Rect(nodeData.pos, defultNodeSize));
                }

                if(nodeData.type == FAED_TreeNodeType.Tree)
                {

                    var node = treeGraph.CreateNode(nodeData.text, FAED_TreeNodeType.Tree, defultNodeSize);
                    node.GUID = nodeData.GUID;

                    treeGraph.CreatePort(node, "Before", Direction.Input);
                    treeGraph.CreatePort(node, "Next", Direction.Output);

                    node.inputContainer.Add(new Label("Tree"));


                    graphView.AddNode(node);

                    node.SetPosition(new Rect(nodeData.pos, defultNodeSize));
                }

                if(nodeData.type == FAED_TreeNodeType.If)
                {

                    var node = treeGraph.CreateNode(nodeData.text, FAED_TreeNodeType.If, defultNodeSize);
                    node.GUID = nodeData.GUID;

                    treeGraph.CreatePort(node, "Before", Direction.Input);
                    treeGraph.CreatePort(node, "NextTrue", Direction.Output);
                    treeGraph.CreatePort(node, "NextFalse", Direction.Output);

                    node.inputContainer.Add(new Label("If"));

                    graphView.AddNode(node);

                    node.SetPosition(new Rect(nodeData.pos, defultNodeSize));

                }

            }

        }

        private void ConnectModes()
        {

            foreach(var item in data.links)
            {

                var input = nodes.Find(x => x.GUID == item.targetGUID).inputContainer.Q<Port>();
                var output = nodes.Find(x => x.GUID == item.baseGUID).outputContainer.Q<Port>(item.portName);

                LinkNodes(output, input);

            }

        }

        private void LinkNodes(Port output, Port input)
        {

            var tempEdge = new Edge
            {

                output = output,
                input = input

            };

            tempEdge.input.Connect(tempEdge);
            tempEdge.output.Connect(tempEdge);
            graphView.Add(tempEdge);

        }
    }


}
#endif