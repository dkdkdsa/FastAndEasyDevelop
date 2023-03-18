using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Graphs;
using UnityEditor;
using UnityEditorInternal;

public class AnimeGraphTest : EditorWindow
{

    private static AnimeGraphTest ins;

    [MenuItem("FAED_AI/TestAnime")]
    private static void SER()
    {

        ins = CreateWindow<AnimeGraphTest>();
        ins.Show();

    }

    private void OnGUI()
    {

        var graph = CreateInstance<Graph>();
        var node = CreateInstance<Node>();
        graph.AddNode(node);

        var gui = CreateInstance<FTestGraphGUI>();

        gui.BeginGraphGUI(ins, new Rect(100, 100, 100, 100));
        gui.graph = graph;
        gui.OnGraphGUI();

    }

    public class FTestGraphGUI : GraphGUI
    {



    }

}
