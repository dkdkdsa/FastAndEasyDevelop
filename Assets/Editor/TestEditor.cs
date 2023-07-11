using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FD.Core.Editors;
using UnityEditor;

public class TestEditor : FAED_GraphBaseWindow<FAED_BaseGraphView>
{

    [MenuItem("TEST/TestGrp")]
    private static void Open()
    {

        var win = GetWindow<TestEditor>();
        win.Show();

    }

    protected override void OnEnable()
    {

        base.OnEnable();
        AddToolBar();
        graphView.SetZoom();
        graphView.SetDrag();
        graphView.SetMiniMap(new Rect(10, 30, 300, 300));
        graphView.AddNode<FAED_BaseNode>();
        graphView.SetGrid();

    }

}
