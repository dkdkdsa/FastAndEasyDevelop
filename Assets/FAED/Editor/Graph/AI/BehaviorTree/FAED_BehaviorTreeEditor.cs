using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

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
            graphView.AddNode<FAED_BehaviorTreeStartNode>("StartPoint", new Vector2(300, 300), new Vector2(100, 100), true, false);

        }

    }

    public class FAED_BehaviorTreeGraph : FAED_BaseGraphView
    {

        private FAED_BehaviorTreeSearchWindow searchWindow;

        public FAED_BehaviorTreeGraph()
        {


            

        }

        private void AddSearchWindow()
        {

            searchWindow = ScriptableObject.CreateInstance<FAED_BehaviorTreeSearchWindow>();
            nodeCreationRequest = v => 

        }

    }

    public class FAED_BehaviorTreeStartNode : FAED_BaseNode
    {

        public FAED_BehaviorTreeStartNode()
        {

            titleContainer.style.backgroundColor = (Color)new Color32(150, 0, 0, 255);

            var portAddButton = new Button(HandlePortAddButton);

            portAddButton.text = "+";

            titleButtonContainer.Add(portAddButton);
        
        }

        private void HandlePortAddButton()
        {

            var port = AddPort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
            port.portName = $"{outputContainer.childCount}";

            var portRemoveButton = new Button(() => HandlePortRemoveButton(port));

            portRemoveButton.text = "X";

            port.Add(portRemoveButton);

        }

        private void HandlePortRemoveButton(Port port)
        {

            outputContainer.Remove(port);

            var portList = outputContainer.Query<Port>().ToList();

            for(int i = 0; i < outputContainer.childCount; i++)
            {

                portList[i].portName = (i + 1).ToString();

            }

        }

    }

    public class FAED_BehaviorTreeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {

            var tree = new List<SearchTreeEntry>()
            {

                new SearchTreeGroupEntry(new GUIContent("Create Behavior"), 0)

            };

            return tree;

        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {

            return true;

        }

    }

}