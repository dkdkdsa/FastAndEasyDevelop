using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FD.System.Editor.Graph;
using UnityEditor;

public class TestView : FAED_GraphBaseWindow<TestView>
{

    [MenuItem("Test/View")]
    protected static void ShowWindow()
    {

        CreateGraphWindow();
        window.Show();
        Set();
        
    }

    private static void Set()
    {

        var node = new FAED_GraphNode();
        
        window.Add(node);
        var d = new FAED_NodeDragEvent(node);

    }

}
