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

    internal class FAED_TreeGraph
    {

        private FAED_GraphView graphView;

        internal FAED_TreeGraph(FAED_GraphView graphView)
        {

            this.graphView = graphView;

        }

        internal FAED_TreeGraphNode CreateNode(string name, FAED_TreeNodeType type, Vector2 size, bool moveAble = true, bool deleteAble = true, bool nameChangeAble = true)
        {

            var node = new FAED_TreeGraphNode
                (
                    
                    Guid.NewGuid().ToString(),
                    name,
                    type
                    
                );

            if (!moveAble)
            {

                node.capabilities &= ~Capabilities.Movable;

            }

            if (!deleteAble)
            {

                node.capabilities &= ~Capabilities.Deletable;

            }

            if (nameChangeAble)
            {

                node.title = name;
                var textField = new TextField(string.Empty);
                textField.RegisterValueChangedCallback(evt =>
                {

                    node.text = evt.newValue;
                    node.title = evt.newValue;

                });
                textField.SetValueWithoutNotify(node.title);
                node.mainContainer.Add(textField);

            }
            else
            {

                node.title = name;

            }

            node.RefreshExpandedState();
            node.RefreshPorts();
            node.SetPosition(new Rect(position: Vector2.zero, size));

            return node;

        }

        internal Button CreatePortAddButton(FAED_TreeGraphNode node, Port.Capacity capacity = Port.Capacity.Single)
        {

            var button = new Button(() =>
            {

                int count = node.outputContainer.Query("connector").ToList().Count;
                var port = CreatePort(node, $"{count + 1}", Direction.Output, capacity);

                var delButton = new Button(() => RemovePort(node, port));
                delButton.text = "X";
                port.contentContainer.Add(delButton);

                node.RefreshExpandedState();
                node.RefreshPorts();

            });

            button.text = "AddPort";


            node.titleButtonContainer.Add(button);


            return button;

        }

        internal Port CreatePort(FAED_TreeGraphNode node,string portName, Direction direction, Port.Capacity capacity = Port.Capacity.Single)
        {

            var port = node.InstantiatePort(Orientation.Horizontal, direction, capacity, typeof(float));
            port.portName = portName;
            port.name = portName;

            if(direction == Direction.Input)
            {

                node.inputContainer.Add(port);

            }
            else
            {

                node.outputContainer.Add(port);

            }

            node.RefreshExpandedState();
            node.RefreshPorts();

            return port;

        }

        internal Port CreateButtonPort(FAED_TreeGraphNode node,string name)
        {

            var port = CreatePort(node, name,Direction.Output);

            var delButton = new Button(() => RemovePort(node, port));
            delButton.text = "X";
            port.contentContainer.Add(delButton);

            node.RefreshExpandedState();
            node.RefreshPorts();

            return port;

        }

        private void RemovePort(FAED_TreeGraphNode node, Port port)
        {

            var targetEdge = graphView.edges.ToList().Where(x => x.output.portName == port.portName && x.output.node == port.node);

            if (targetEdge.Any())
            {

                var edge = targetEdge.First();
                edge.input.Disconnect(edge);
                graphView.RemoveElement(targetEdge.First());

            }


            node.outputContainer.Remove(port);

            node.RefreshPorts();
            node.RefreshExpandedState();

            var item = node.outputContainer.Query<Port>().ToList().OrderBy(x => int.Parse(x.portName)).ToList();

            for(int i = 0; i < item.Count; i++)
            {

                item[i].portName = (i + 1).ToString();

            }

            node.RefreshPorts();
            node.RefreshExpandedState();

        }

    }

}
#endif