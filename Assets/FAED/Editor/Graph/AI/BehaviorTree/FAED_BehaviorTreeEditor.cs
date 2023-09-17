using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Linq;
using System.IO;
using System;
using FD.Dev.AI;


namespace FD.Core.Editors
{


    //그래프뷰 에디터 창
    internal class FAED_BehaviorTreeBaseEditor : FAED_GraphBaseWindow<FAED_BehaviorTreeGraph>
    {

        private VisualElement graphRoot;
        private FAED_BehaviorTree behaviorTree;

        [MenuItem("FAED/AI/BehaviorTree")]
        private static void Open()
        {

            var window = CreateWindow<FAED_BehaviorTreeBaseEditor>();
            window.titleContent.text = "BehaviorTree Editor";
            window.maximized = true;
            window.Show();

        }

        protected override void OnEnable()
        {

            AddToolBar();
            SetUpToolbar();
            CreateGraphRoot();
            SetUpGraphView();
            CreateSpliteView();

            graphView.AddElement(new FAED_BehaviorRootNode(graphView));

        }

        private void CreateGraphRoot()
        {

            graphRoot = new VisualElement();
            graphRoot.style.position = Position.Relative;
            graphRoot.style.flexDirection = FlexDirection.Row;
            graphRoot.style.flexGrow = 1;

            rootVisualElement.Add(graphRoot);

        }

        private void CreateSpliteView()
        {

            TwoPaneSplitView splitView = new TwoPaneSplitView(1, 300, TwoPaneSplitViewOrientation.Horizontal);


            splitView.contentContainer.Add(graphView);
            splitView.contentContainer.Add(new FAED_VisualWindow("Insp", Position.Relative, new Color(0.2f, 0.2f, 0.2f)));
            graphRoot.Add(splitView);

        }

        private void SetUpGraphView()
        {

            graphView = new FAED_BehaviorTreeGraph();

            graphView.SetDrag();
            graphView.SetMiniMap(new Rect(10, 30, 300, 300));
            graphView.SetGrid();
            graphView.SetZoom();

            graphView.style.position = Position.Relative;
            graphView.style.width = 1600;
            graphView.style.flexGrow = 1;

        }

        private void SetUpToolbar()
        {

            toolbar.Add(new Button(Save));
            toolbar.Add(new Button(Load));

        }

        private void Load()
        {



        }

        private void Save()
        {

            if(behaviorTree == null)
            {

                var path = EditorUtility.SaveFilePanel("SaveBehaviorTree", Application.dataPath, "NewBehaviorTree", "assets");
                behaviorTree = ScriptableObject.CreateInstance<FAED_BehaviorTree>();
                
                AssetDatabase.CreateAsset(behaviorTree, path);

            }

            var nodeLs = graphView.Query<FAED_BehaviorTreeNode>().ToList();

            foreach(var node in nodeLs)
            {

                var obj = ScriptableObject.CreateInstance(node.classType);
                AssetDatabase.AddObjectToAsset(obj, behaviorTree);

                behaviorTree.nodes.Add(obj as FAED_Node);

            }

        }

    }

    internal class FAED_BehaviorTreeGraph : FAED_BaseGraphView
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

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {

            return ports.ToList().Where(x => x.direction != startPort.direction && x.node != startPort.node).ToList();

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

    internal class FAED_BehaviorTreeSearchWindow : ScriptableObject, ISearchWindowProvider
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
                new SearchTreeGroupEntry(new GUIContent("Node"), 1),

            };

            var types = TypeCache.GetTypesDerivedFrom<FAED_Node>();

            var nodes = types.Where(x => x.IsAbstract == false && x.Name != "FAED_RootNode").ToList();

            for (int i = 0; i < nodes.Count; i++)
            {

                tree.Add(new SearchTreeEntry(new GUIContent(nodes[i].Name)) { userData = nodes[i], level = 2 });

            }

            return tree;

        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {


            if(searchTreeEntry.userData is Type)
            {

                var t = searchTreeEntry.userData as Type;

                if (t.IsSubclassOf(typeof(FAED_ControlFlowNode)))
                {

                    var node = new FAED_BehaviorChildNode(graph, t.Name, "ControlFlow", t);
                    node.AddPort(Orientation.Vertical, Direction.Input, Port.Capacity.Single);
                    node.AddPort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi);
                    node.transform.position = context.screenMousePosition;

                    graph.AddElement(node);

                }
                else if(t.IsSubclassOf(typeof(FAED_DecoratorNode)))
                {

                    var node = new FAED_BehaviorChildNode(graph, t.Name, "Decorator", t);
                    node.AddPort(Orientation.Vertical, Direction.Input, Port.Capacity.Single);
                    node.AddPort(Orientation.Vertical, Direction.Output, Port.Capacity.Single);
                    node.transform.position = context.screenMousePosition;

                    graph.AddElement(node);

                }
                else
                {

                    var node = new FAED_BehaviorActionNode(t.Name, "Behavior", t);
                    node.AddPort(Orientation.Vertical, Direction.Input, Port.Capacity.Single);
                    node.transform.position = context.screenMousePosition;

                    graph.AddElement(node);


                }

            }

            return false;

        }

    }

    internal class FAED_BehaviorTreeNode : FAED_BaseNode
    {

        public Type classType;

        public FAED_BehaviorTreeNode() : base(AssetDatabase.GetAssetPath(Resources.Load<VisualTreeAsset>("BehaviorNode")))
        {

            var style = Resources.Load<StyleSheet>("BehaviorNodeStyle");

            styleSheets.Add(style);
            AddToClassList("node");

            RefreshAll();

        }

    }

    internal class FAED_BehaviorActionNode : FAED_BehaviorTreeNode
    {        

        public FAED_BehaviorActionNode(string className, string message, Type classType) : base()
        {

            title = className;
            mainContainer.Q<Label>("description").text = message;
            this.classType = classType;

        }

    }

    internal class FAED_BehaviorRootNode : FAED_BehaviorChildNode
    {

        public FAED_BehaviorRootNode(FAED_BehaviorTreeGraph graph) : base(graph, "StartPoint", "", typeof(FAED_RootNode))
        {

            AddPort(Orientation.Vertical, Direction.Output, Port.Capacity.Single);

        }

    }

    internal class FAED_BehaviorChildNode : FAED_BehaviorTreeNode
    {

        protected FAED_BehaviorTreeGraph graph;

        public FAED_BehaviorChildNode(FAED_BehaviorTreeGraph graph, string title, string message, Type classType) : base()
        {

            this.title = title;
            mainContainer.Q<Label>("description").text = message;
            this.classType = classType;
            this.graph = graph;

        }

        public List<GUID> ConnectNode()
        {

            var targetEdge = graph.edges.ToList().Where(x => x.output.node == this).Select(x => x.input.node).ToList();

            return targetEdge.OrderBy(x => x.transform.position.x).ToList().ConvertAll(x => (x as FAED_BehaviorChildNode).guid);

        }

    }

    internal class FAED_VisualWindow : VisualElement
    {

        public VisualElement titleContainer { get; protected set; }
        public Label titleLabel { get; protected set; }

        public FAED_VisualWindow(string text, Position position, Color backGroundColor)
        {

            style.backgroundColor = backGroundColor;
            style.position = position;

            CreateTitleContainer();

            titleLabel = new Label(text);
            
            
            titleContainer.Add(titleLabel);

        }

        private void CreateTitleContainer()
        {

            titleContainer = new VisualElement();
            titleContainer.style.position = Position.Relative;
            titleContainer.style.backgroundColor = Color.black;
            titleContainer.style.flexShrink = 0;

            Add(titleContainer);

        }

    }

}