#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using FD.UI.Tool;

namespace FD.AI.FSM
{
    
    public class FAED_FSMSave
    {

        private FAED_FSMGraph fsmGraph;
        private FAED_FSMGraphViewWindow window;
        private FAED_FSMSaveSO data;

        private List<Edge> edges => fsmGraph.edges.ToList();
        private List<FAED_FSMNode> nodes => fsmGraph.nodes.ToList().Cast<FAED_FSMNode>().ToList();

        public static FAED_FSMSave GetInstance(FAED_FSMGraph fsmGraph, FAED_FSMGraphViewWindow window)
        {

            return new FAED_FSMSave
            {

                fsmGraph = fsmGraph,
                window = window

            };

        }

        public void SaveGraph(string fileName)
        {

            if(!edges.Any()) return;

            var data = ScriptableObject.CreateInstance<FAED_FSMSaveSO>();
            var connectedPorts = edges.Where(x => x.input.node != null).ToList();

            foreach (var item in connectedPorts)
            {

                var output = item.output.node as FAED_FSMNode;
                var input = item.input.node as FAED_FSMNode;

                data.linkData.Add(new FAEDE_FSMLinkData
                {

                    baseGuid = output.GUID,
                    portName = item.output.portName,
                    targetGuid = input.GUID,

                });

            }

            foreach (var dataNode in nodes)
            {

                data.nodeData.Add(new FAED_FSMSaveData
                {

                    guid = dataNode.GUID,
                    text = dataNode.title,
                    pos = dataNode.GetPosition().position,
                    type = dataNode.nodeType,
                    ports = dataNode.outputContainer.Query<Port>().ToList().Select(x => x.portName).ToList(),
                    inputPorts = dataNode.inputContainer.Query<Port>().ToList().Select(x => x.portName).ToList()

                });

            }

            AssetDatabase.CreateAsset(data, $"Assets/Resources/FAED/FSM/{fileName}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();


        }

        public void LoadGraph(string fileName)
        {

            data = Resources.Load<FAED_FSMSaveSO>($"FAED/FSM/{fileName}");

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

                edges.Where(x => x.input.node == node).ToList().ForEach(edge => fsmGraph.RemoveElement(edge));

                fsmGraph.RemoveElement(node);

            }

        }

        private void CreateNodes()
        {

            foreach(var node in data.nodeData)
            {

                if(node.type == FAED_FSMNodeType.Root)
                {

                    var rootNode = new FAED_FSMNode(node.guid, FAED_FSMNodeType.Root);
                    var portBtn = rootNode.titleButtonContainer.AddButton(() =>
                    {

                        var port = rootNode.AddPort((rootNode.outputContainer.Query("connector").ToList().Count + 1).ToString(),
                                        Direction.Output, Port.Capacity.Single);

                        var btn = port.AddButton(() =>
                        {

                            window.RemovePort(rootNode, port, port.direction);
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

                    foreach(var item in node.ports)
                    {

                        var port = rootNode.AddPort(item, Direction.Output, Port.Capacity.Single);

                        var btn = port.AddButton(() =>
                        {

                            window.RemovePort(rootNode, port, port.direction);
                            var lst = rootNode.outputContainer.Query<Port>().ToList();

                            for (int i = 0; i < lst.Count; i++)
                            {

                                lst[i].portName = (i + 1).ToString();
                                lst[i].name = (i + 1).ToString();

                            }

                        });

                        btn.text = "X";

                        btn.text = "AddPort";


                    }

                    window.CreateNode(rootNode, new Vector2(100, 100), "RootNode", true, false);
                    window.AddNode(rootNode);
                    rootNode.SetPosition(new Rect(node.pos, new Vector2(100, 100)));

                }

                if(node.type == FAED_FSMNodeType.State)
                {

                    FAED_FSMNode itemNode = new FAED_FSMNode(node.guid, FAED_FSMNodeType.State);

                    window.CreateNode(itemNode, new Vector2(100, 100), node.text);
                    itemNode.AddNameChangeEvent();

                    itemNode.AddPort("EnderState", Direction.Input, Port.Capacity.Single);

                    var portBtn = itemNode.titleButtonContainer.AddButton(() =>
                    {

                        var port = itemNode.AddPort((itemNode.outputContainer.Query("connector").ToList().Count + 1).ToString(),
                                        Direction.Output, Port.Capacity.Single);

                        var btn = port.AddButton(() =>
                        {

                            window.RemovePort(itemNode, port, port.direction);
                            var lst = itemNode.outputContainer.Query<Port>().ToList();

                            for (int i = 0; i < lst.Count; i++)
                            {

                                lst[i].portName = (i + 1).ToString();
                                lst[i].name = (i + 1).ToString();

                            }

                        });

                        btn.text = "X";

                    });

                    foreach (var item in node.ports)
                    {

                        var port = itemNode.AddPort(item, Direction.Output, Port.Capacity.Single);

                        var btn = port.AddButton(() =>
                        {

                            window.RemovePort(itemNode, port, port.direction);
                            var lst = itemNode.outputContainer.Query<Port>().ToList();

                            for (int i = 0; i < lst.Count; i++)
                            {

                                lst[i].portName = (i + 1).ToString();
                                lst[i].name = (i + 1).ToString();

                            }

                        });

                        btn.text = "X";

                        btn.text = "AddPort";

                    }

                    portBtn.text = "AddPort";

                    window.AddNode(itemNode);
                    itemNode.SetPosition(new Rect(node.pos, new Vector2(100, 100)));

                }

                if(node.type == FAED_FSMNodeType.Transition)
                {

                    var itemNode = new FAED_FSMNode(node.guid, FAED_FSMNodeType.Transition);
                    itemNode.AddNameChangeEvent();
                    window.CreateNode(itemNode, new Vector2(100, 100), node.text);
                    itemNode.AddPort("State", Direction.Input, Port.Capacity.Single);
                    window.AddNode(itemNode);
                    itemNode.mainContainer.Q<TextField>().label = "GoTo";
                    itemNode.SetPosition(new Rect(node.pos, new Vector2(100, 100)));
                }

            }

        }
        private void ConnectModes()
        {

            foreach (var item in data.linkData)
            {

                var input = nodes.Find(x => x.GUID == item.targetGuid).inputContainer.Q<Port>();
                var output = nodes.Find(x => x.GUID == item.baseGuid).outputContainer.Q<Port>(item.portName);

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
            fsmGraph.Add(tempEdge);

        }
    }

}
#endif