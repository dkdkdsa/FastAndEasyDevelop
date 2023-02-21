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

namespace FD.AI.Tree.Node
{

    public class FAED_TreeGraphNode : UnityEditor.Experimental.GraphView.Node
    {

        public string GUID;
        public string text;
        public FAED_TreeNodeType type;

        public FAED_TreeGraphNode(string GUID, string text, FAED_TreeNodeType type)
        {

            this.GUID = GUID;
            this.text = text;
            this.type = type;

        }
    }

    public enum FAED_TreeNodeType
    {

        Root,
        Sequence,
        Tree,
        If,

    }

}

