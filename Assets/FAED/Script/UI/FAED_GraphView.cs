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
    public class FAED_GraphViewWindow : EditorWindow
    {

        protected FAED_GraphView graphView;
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

            graphView = new FAED_GraphView() { name = graphViewName };

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

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {

            var compatiblePorts = new List<Port>();

            ports.ForEach(funcCall: (port) =>
            {

                if (startPort != port && startPort.node != port.node)
                {

                    var start = startPort.node.Q<FAED_TreeGraphNode>().type;
                    var end = port.node.Q<FAED_TreeGraphNode>().type;

                    if(!((start == FAED_TreeNodeType.Tree || start == FAED_TreeNodeType.If) && end == FAED_TreeNodeType.Sequence))
                    {

                        compatiblePorts.Add(port);

                    }


                }

            });

            return compatiblePorts;

        }

        public void AddNode(GraphElement element)
        {

            AddElement(element);

        }

    }

}
