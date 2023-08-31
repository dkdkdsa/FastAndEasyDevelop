using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Linq;

namespace FD.Core.Editors
{

    //그래프뷰 에디터 창
    public class FAED_BehaviorTreeBaseEditor : FAED_GraphBaseWindow<FAED_BehaviorTreeGraph>
    {

        [MenuItem("FAED/AI/BehaviorTree")]
        private static void Open()
        {

            var window = GetWindow<FAED_BehaviorTreeBaseEditor>();
            window.titleContent.text = "BehaviorTree Editor";
            window.maximized = true;
            window.Show();

        }

        protected override void OnEnable()
        {

            base.OnEnable();
            graphView.SetDrag();
            graphView.SetMiniMap(new Rect(10, 30, 300, 300));
            graphView.SetGrid();
            graphView.SetZoom();
            var startPointNode = graphView.AddNode<FAED_BehaviorTreeStartNode>("StartPoint", new Vector2(300, 300), new Vector2(100, 100), true, false);
            startPointNode.Init(graphView);

        }

    }

    public class FAED_BehaviorTreeGraph : FAED_BaseGraphView
    {

        private FAED_BehaviorTreeSearchWindow searchWindow;

        public FAED_BehaviorTreeGraph()
        {

            AddSearchWindow();

        }

        private void AddSearchWindow()
        {

            searchWindow = ScriptableObject.CreateInstance<FAED_BehaviorTreeSearchWindow>();
            searchWindow.Init(this);
            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);

        }

        public FAED_BehaviorTreeNode CreateBehaviorNode(Vector2 pos)
        {

            return AddNode<FAED_BehaviorTreeNode>("New Behavior Node", new Vector2(100, 100), pos);

        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {

            var compPort = new List<Port>();

            ports.ForEach(port =>
            {

                if(startPort != port && startPort.node != port.node)
                {

                    compPort.Add(port);

                }

            });

            return compPort;

        }

        public void RemovePort(FAED_BaseNode node, Port port)
        {

            var targetEdge = edges.ToList().Where(x => x.output.node == port.node && x.output.portName == port.portName);

            if (targetEdge.Any())
            {

                var edge = targetEdge.First();
                edge.input.Disconnect(edge);
                RemoveElement(targetEdge.First());

            }
            
            node.outputContainer.Remove(port);

            node.RefreshAll();

            var item = node.outputContainer.Query<Port>().ToList().OrderBy(x => int.Parse(x.portName)).ToList();

            for (int i = 0; i < item.Count; i++)
            {

                item[i].portName = (i + 1).ToString();

            }

            node.RefreshAll();

        }

    }

    public class FAED_BehaviorTreeSearchWindow : ScriptableObject, ISearchWindowProvider
    {

        private FAED_BehaviorTreeGraph graph;

        public void Init(FAED_BehaviorTreeGraph graph)
        {

            this.graph = graph;

        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {

            var tree = new List<SearchTreeEntry>()
            {

                new SearchTreeGroupEntry(new GUIContent("Create Behavior"), 0),
                new SearchTreeGroupEntry(new GUIContent("Behavior"), 1),
                new SearchTreeEntry(new GUIContent("BehaviorNode")){userData = "BehaviorNode", level = 2}

            };

            return tree;

        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {

            switch (searchTreeEntry.userData)
            {

                case "BehaviorNode":
                    {

                        var node = graph.CreateBehaviorNode(context.screenMousePosition);
                        node.Init(graph);

                    }

                    return true;
                default:
                    return false;

            }

        }

    }

    public class FAED_BehaviorTreeStartNode : FAED_BaseNode
    {

        private FAED_NodePortCreater portCreater;

        public FAED_BehaviorTreeStartNode()
        {

            titleContainer.style.backgroundColor = (Color)new Color32(150, 0, 0, 255);

        
        }

        public void Init(FAED_BehaviorTreeGraph graphView)
        {

            portCreater = new FAED_NodePortCreater(this, graphView, "+");

        }

    }

    public class FAED_BehaviorTreeNode : FAED_BaseNode
    {

        private FAED_NodePortCreater portCreater;
        //클래스이름
        public string className;


        public FAED_BehaviorTreeNode()
        {

            titleContainer.style.backgroundColor = (Color)new Color32(0, 0, 150, 255);

            var inputPort = AddPort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single);
            inputPort.portName = "";

        }

        public void Init(FAED_BehaviorTreeGraph graphView)
        {

            portCreater = new FAED_NodePortCreater(this, graphView, "+");

        }


    }

    internal class FAED_NodePortCreater
    {

        private FAED_BaseNode node;
        private FAED_BehaviorTreeGraph graphView;

        public Button portAddButton { get; private set; }

        internal FAED_NodePortCreater(FAED_BaseNode node, FAED_BehaviorTreeGraph graphView, string buttonText = "")
        {

            this.node = node;
            this.graphView = graphView;

            portAddButton = new Button(HandlePortAddButton);
            portAddButton.text = buttonText;
            node.titleButtonContainer.Add(portAddButton);

        }

        private void HandlePortAddButton()
        {

            var port = node.AddPort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
            port.portName = $"{node.outputContainer.childCount}";

            var portRemoveButton = new Button(() => HandlePortRemoveButton(port));

            portRemoveButton.text = "X";
            portRemoveButton.style.backgroundColor = (Color)new Color32(255, 51, 51, 255);

            port.Add(portRemoveButton);

        }
        private void HandlePortRemoveButton(Port port)
        {

            graphView.RemovePort(node, port);

            node.RefreshAll();

        }

    }

}