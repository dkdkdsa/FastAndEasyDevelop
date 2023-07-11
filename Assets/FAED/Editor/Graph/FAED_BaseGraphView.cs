using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace FD.Core.Editors
{

    public class FAED_BaseGraphView : GraphView
    {

        public FAED_BaseNode AddNode<T>()where T : FAED_BaseNode, new()
        {

            return AddNode<T>("New Node");

        }
        public FAED_BaseNode AddNode<T>(string titleText)where T : FAED_BaseNode, new()
        {

            return AddNode<T>(titleText, new Vector2(300, 300));

        }
        public FAED_BaseNode AddNode<T>(string titleText, Vector2 size)where T : FAED_BaseNode, new()
        {

            return AddNode<T>(titleText, size, Vector2.zero);

        }
        public FAED_BaseNode AddNode<T>(string titleText, Vector2 size, Vector2 position)where T : FAED_BaseNode, new()
        {

            return AddNode<T>(titleText, size, position, true, true);

        }
        public FAED_BaseNode AddNode<T>(string titleText, Vector2 size, Vector2 position, bool movable, bool deletable)where T : FAED_BaseNode, new()
        {

            var node = new T();

            if (!movable) node.capabilities &= ~Capabilities.Movable;
            if (!deletable) node.capabilities &= ~Capabilities.Deletable;

            node.title = titleText;
            node.SetPosition(new Rect(position, size));
            node.RefreshAll();

            AddElement(node);

            return node;

        }

    }

}