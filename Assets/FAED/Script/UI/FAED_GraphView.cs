using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using FD.AI.Tree.Node;

namespace FD.UI
{
    public class FAED_GraphViewWindow<T> : EditorWindow where T : GraphView, new()
    {

        protected T graphView;
        protected Toolbar toolbar;
        protected MiniMap miniMap;

        public void CreateWindowElement(string graphViewName, bool miniMap = false, bool toolBar = false)
        {

            CreateGraphView(graphViewName);

            if(miniMap) CreateMiniMap();
            if(toolBar) CreateToolBar();

        }

        private void CreateGraphView(string graphViewName)
        {

            graphView = new T() { name = graphViewName };

            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);

        }

        private void CreateMiniMap()
        {

            miniMap = new MiniMap { anchored = true };
            miniMap.SetPosition(new Rect(10, 30, 200, 140));
            graphView.Add(miniMap);

        }

        private void CreateToolBar()
        {

            toolbar = new Toolbar();
            rootVisualElement.Add(toolbar);

        }

    }

    public class FAED_GraphView : GraphView
    {

        public FAED_GraphView()
        {

            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var guid = new GridBackground();
            Insert(index: 0, guid);
            guid.StretchToParentSize();

        }

        public void AddNode(GraphElement element)
        {

            AddElement(element);

        }

    }

}
