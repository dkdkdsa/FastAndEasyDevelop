using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Linq;
using System.IO;

namespace FD.Core.Editors
{

    //그래프뷰 에디터 창
    internal class FAED_BehaviorTreeBaseEditor : FAED_GraphBaseWindow<FAED_BehaviorTreeGraph>
    {

        private VisualElement graphRoot;

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


            AddToolBar();
            CreateGraphRoot();
            SetUpGraphView();
            CreateSpliteView();
            ///
            var startPointNode = graphView.AddNode<FAED_BehaviorTreeStartNode>("StartPoint", new Vector2(300, 300), new Vector2(100, 100), true, false);
            startPointNode.Init(graphView);

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
            TwoPaneSplitView sysSplit = new TwoPaneSplitView(1, 300, TwoPaneSplitViewOrientation.Vertical); ;

            sysSplit.contentContainer.Add(new FAED_CreateClassWindow());
            sysSplit.contentContainer.Add(new FAED_VisualWindow("Insp", Position.Relative, new Color(0.2f, 0.2f, 0.2f)));

            splitView.contentContainer.Add(graphView);
            splitView.contentContainer.Add(sysSplit);
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

        public FAED_BehaviorTreeNode CreateBehaviorNode(Vector2 pos)
        {

            return AddNode<FAED_BehaviorTreeNode>("New Behavior Node", new Vector2(100, 100), pos);

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

    internal class FAED_BehaviorTreeStartNode : FAED_BaseNode
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

    internal class FAED_BehaviorTreeNode : FAED_BaseNode
    {

        private FAED_NodePortCreater portCreater;
        //클래스이름
        public string className;


        public FAED_BehaviorTreeNode()
        {

            titleContainer.style.backgroundColor = (Color)new Color32(0, 0, 150, 255);

            var inputPort = AddPort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single);
            inputPort.portName = "(BehaviorNode)";

            var portLabel = inputPort.Q<Label>();
            portLabel.style.color = (Color)new Color32(102, 102, 102, 255);
            portLabel.style.unityFontStyleAndWeight = FontStyle.Italic;

            var nameTextField = new TextField(title);
            nameTextField.RegisterValueChangedCallback(evt =>
            {

                title = evt.newValue;

            });

            mainContainer.Add(nameTextField);

        }

        public void Init(FAED_BehaviorTreeGraph graphView)
        {

            portCreater = new FAED_NodePortCreater(this, graphView, "+");

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

    internal class FAED_CreateClassWindow : FAED_VisualWindow
    {

        private ScrollView scrollView;

        private const string DefaultScriptFormat =
@"using UnityEngine;
        
public class {0}
{{
    
    public @{1}_Value value;

}}";

        private const string DefaultValueStructFormat =
@"using UnityEngine;

public struct @{0}_Value
{{

}}";

        public FAED_CreateClassWindow() : base("NodeClass", Position.Relative, new Color(0.2f, 0.2f, 0.2f))
        {

            SetTitleContainer();
            SetClassAddBtn();
            SetScrollView();

        }

        private void SetTitleContainer()
        {

            titleContainer.style.flexDirection = FlexDirection.Row;
            titleContainer.style.justifyContent = Justify.SpaceBetween;

        }

        private void SetClassAddBtn()
        {

            var btn = new Button(HandleClassCreateButtonClick);
            btn.text = "+";
            titleContainer.Add(btn);

        }

        private void SetScrollView()
        {

            scrollView = new ScrollView();
            Add(scrollView);

        }

        private void HandleClassCreateButtonClick()
        {

            string path = EditorUtility.SaveFilePanel("Create Script", Application.dataPath, "NewScript", "cs");

            if(path != string.Empty)
            {

                var fileNameArr = path.Split('/');
                var fileName = fileNameArr[fileNameArr.Length - 1].Split('.')[0];
                fileName.Replace(' ', '_');

                File.WriteAllText(path, string.Format(DefaultScriptFormat, fileName));

                scrollView.contentContainer.Add(new FAED_ClassPanel(fileName, path));

            }

        }

    }

    internal class FAED_ClassPanel : VisualElement
    {

        public Label titleLable { get; private set; }
        public string filePath { get; private set; }

        public FAED_ClassPanel(string textTitle, string filePath)
        {

            this.filePath = filePath;
            titleLable = new Label(textTitle);

            style.backgroundColor = new Color(0.1f, 0.1f, 0.1f);

            style.height = 30;
            style.flexGrow = 0;
            style.flexShrink = 0;

            style.borderBottomLeftRadius = 10;
            style.borderBottomRightRadius = 10;
            style.borderTopLeftRadius = 10;
            style.borderTopRightRadius = 10;

            style.borderBottomColor = Color.white;
            style.borderLeftColor = Color.white;
            style.borderRightColor = Color.white;
            style.borderTopColor = Color.white;

            style.borderRightWidth = 2;
            style.borderBottomWidth = 2;
            style.borderLeftWidth = 2;
            style.borderTopWidth = 2;

            style.alignItems = Align.Center;
            style.justifyContent = Justify.Center;

            style.marginBottom = 10;

            Add(titleLable);
            this.filePath = filePath;
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