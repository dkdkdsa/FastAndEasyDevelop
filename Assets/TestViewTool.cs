using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FD.UI.Tool;
using UnityEditor;
using FD.AI.Tree;
using UnityEditor.Experimental.GraphView;
using FD.UI;

public class TestViewTool : FAED_GraphViewTool<FAED_GraphView>
{


    [MenuItem("FAED_AI/Test")]
    private static void CreateWindows()
    {

        var window = GetWindow<TestViewTool>();
        window.titleContent = new GUIContent("Test");

    }

    private void OnEnable()
    {

        CreateWindowElement("Test", true, true);

        var node = CreateNode<Node>("asdf", new Vector2(300, 300), false, false);
        node.AddPort("1", Direction.Input, Port.Capacity.Single);

        graphView.AddNode(node);


    }

}
