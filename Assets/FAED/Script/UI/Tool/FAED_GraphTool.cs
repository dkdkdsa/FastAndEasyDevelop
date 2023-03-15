#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

namespace FD.UI.Tool
{

    public class FAED_GraphViewTool : FAED_GraphViewWindow
    {

        public T CreateNode<T>(string titleText, Vector2 size, bool moveAble = true, bool deleteAble = true) where T : Node, new()
        {

            T node = new T();

            if (!moveAble) 
            {

                node.capabilities &= ~Capabilities.Movable;

            }

            if (!deleteAble)
            {

                node.capabilities &= ~Capabilities.Deletable;

            }

            node.title = titleText;

            node.RefreshExpandedState();
            node.RefreshPorts();
            node.SetPosition(new Rect(position: Vector2.zero, size));

            return node;

        }

        public T CreateNode<T>(T nodeObject, Vector2 size, string titleText, bool moveAble = true, bool deleteAble = true) where T : Node
        {

            nodeObject.title = titleText;

            if (!moveAble)
            {

                nodeObject.capabilities &= ~Capabilities.Movable;

            }

            if (!deleteAble)
            {

                nodeObject.capabilities &= ~Capabilities.Deletable;

            }

            nodeObject.RefreshExpandedState();
            nodeObject.RefreshPorts();
            nodeObject.SetPosition(new Rect(position: Vector2.zero, size));

            return nodeObject;

        }

    }

    public static class FAED_GraphViewSupportTool
    {

        public static TextField AddTextField(this VisualElement element)
        {

            TextField textField = new TextField();

            element.Add(textField);

            return textField;

        }

        public static void AddNameChangeEvnet(this TextField textField, Node node) 
        {

            textField.RegisterValueChangedCallback(evt =>
            {

                node.title = evt.newValue;

            });

            textField.SetValueWithoutNotify(node.title);
            node.mainContainer.Add(textField);

            node.RefreshExpandedState();
            node.RefreshPorts();

        }

        public static void AddNameChangeEvent(this Node node)
        {

            TextField textField = node.mainContainer.Q<TextField>();

            if(textField == null)
            {

                node.mainContainer.AddTextField().AddNameChangeEvnet(node);
                node.RefreshExpandedState();
                node.RefreshPorts();
                return;

            }

            textField.RegisterValueChangedCallback(evt =>
            {

                node.title = evt.newValue;

            });

            textField.SetValueWithoutNotify(node.title);
            node.mainContainer.Add(textField);

            node.RefreshExpandedState();
            node.RefreshPorts();


        }

        public static Port AddPort(this Node node, string titleText, Direction direction, Port.Capacity capacity, Orientation orientation = Orientation.Horizontal)
        {

            Port port = node.InstantiatePort(Orientation.Horizontal, direction, capacity, typeof(float));

            port.title = titleText;

            if (direction == Direction.Input)
            {

                node.inputContainer.Add(port);

            }
            else
            {

                node.outputContainer.Add(port);

            }

            return port;

        }

        public static Button AddButton(this VisualElement element, Action clickEvnet)
        {

            Button btn = new Button(clickEvnet);

            element.Add(btn);

            return btn;

        }


    }

}
#endif