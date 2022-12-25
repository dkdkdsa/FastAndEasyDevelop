using FD.Dev;
using FD.Program.Editer.Runtime.Data;
using FD.Program.Editer.SO;
using FD.Program.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;

namespace FD.Program.Runtime
{

    public class FAED_GrahpSave
    {

        private FAED_AIGrahpViewModlue targetView;
        private FAED_DialougeContainer container;

        private List<Edge> edges => targetView.edges.ToList();
        private List<FAED_AIGrahpViewNodeModlue> nodes => targetView.nodes.ToList().Cast<FAED_AIGrahpViewNodeModlue>().ToList();

        public static FAED_GrahpSave GetInstance(FAED_AIGrahpViewModlue targetView)
        {

            return new FAED_GrahpSave
            {

                targetView = targetView

            };

        }

        public void SaveGraph(string fileName)
        {

            if (!edges.Any()) return;

            var logContainer = ScriptableObject.CreateInstance<FAED_DialougeContainer>();

            var connectedPorts  = edges.Where(x => x.input.node != null).ToArray();
           
            foreach(var item in connectedPorts)
            {

                var outputNode = item.output.node as FAED_AIGrahpViewNodeModlue;
                var inputNode = item.input.node as FAED_AIGrahpViewNodeModlue;


                logContainer.links.Add(new FAED_NodeLinkData
                {

                    baseNodeGuid = outputNode.GUID,
                    portName = item.output.portName,
                    targetNodeGuid = inputNode.GUID,
                    portCount = FAED_AIGrahpViewModlue.portNums[new FAED_PortSaveData(outputNode.GUID, item.output.portName)]

                });

            }

            foreach(var logNode in nodes.Where(node => !node.entryPoint))
            {

                logContainer.editorGrahpData.Add(new FAED_EditorGrahpData
                {

                    Guid = logNode.GUID,
                    dialogueText = logNode.viewText,
                    position = logNode.GetPosition().position,
                    nodeType = logNode.nodeType              

                });

            }


            AssetDatabase.CreateAsset(logContainer, $"Assets/Resources/FAED/{fileName}.asset");
            AssetDatabase.SaveAssets();

        }

        public void LoadGraph(string fileName)
        {

            container = Resources.Load<FAED_DialougeContainer>($"FAED/{fileName}");
            if(container == null)
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

            nodes.Find(x => x.entryPoint).GUID = container.links[0].baseNodeGuid;

            foreach(var node in nodes)
            {

                if (node.entryPoint) continue;

                edges.Where(x => x.input.node == node).ToList().ForEach(edge => targetView.RemoveElement(edge));

                targetView.RemoveElement(node);

            }

        }

        private void CreateNodes()
        {
            
            foreach(var nodeData in container.editorGrahpData)
            {

                var tempNode = targetView.CreateDialogNode(nodeData.dialogueText, nodeData.nodeType);
                tempNode.GUID = nodeData.Guid;
                targetView.AddElement(tempNode);

                var nodePorts = container.links.Where(x => x.baseNodeGuid == nodeData.Guid).ToList();
                nodePorts.ForEach(x =>
                {

                    targetView.AddStatePort(tempNode, x.portName);

                });
                

            }

        }

        private void ConnectModes()
        {
            
            for(var i = 0; i < nodes.Count; i++)
            {

                var connections = container.links.Where(x => x.baseNodeGuid == nodes[i].GUID).ToList();
                
                for(var j = 0; j < connections.Count; j++)
                {

                    var targetNodeGuid = connections[j].targetNodeGuid;
                    var targetNode = nodes.First(x => x.GUID == targetNodeGuid);
                    var portCount = connections[j].portCount;

                    LinkNodes(nodes[i].outputContainer[portCount].Q<Port>(), (Port) targetNode.inputContainer[0]);

                    targetNode.SetPosition(new Rect(

                        container.editorGrahpData.First(x => x.Guid == targetNodeGuid).position,
                        targetView.defultNodeSize

                        ));

                }

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
            targetView.Add(tempEdge);

        }
    }

}
