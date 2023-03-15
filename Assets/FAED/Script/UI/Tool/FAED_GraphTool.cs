#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

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

        public static TextField AddTextField(this Node node)
        {

            TextField textField = new TextField();
            textField.SetValueWithoutNotify(node.title);

            node.mainContainer.Add(textField);

            node.RefreshExpandedState();
            node.RefreshPorts();

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

                node.AddTextField().AddNameChangeEvnet(node);
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

    }

}
#endif