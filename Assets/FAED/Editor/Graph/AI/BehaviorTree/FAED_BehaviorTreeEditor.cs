using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Linq;
using System.IO;
using System;
using System.Runtime.Remoting.Contexts;
using UnityEditor.Graphs;


namespace FD.Core.Editors
{

    internal class FAED_BehaviorTreeEditorWindow : FAED_GraphBaseWindow<FAED_BehaviorTreeEditorWindow.FAED_BehaviorTreeGraphView>
    {

        private VisualElement graphRoot;
        private FAED_VisualWindow inspactor;

        [MenuItem("FAED/AI/BehaviorTree")]
        private static void OpenEditor()
        {

            var window = CreateWindow<FAED_BehaviorTreeEditorWindow>();
            window.titleContent.text = "BehaviorTreeEditor";

            window.Show();
            window.maximized = true;

        }

        private void SetUpGraph()
        {

            graphView.SetDrag();
            graphView.SetGrid();
            graphView.SetZoom();
            graphView.style.position = Position.Relative;
            graphView.SetMiniMap(new Rect(10, 10, 300, 300));

        }

        private void SetUpInspacter()
        {

            inspactor = new FAED_VisualWindow("Inspactor", Position.Relative, new Color(0.3f, 0.3f, 0.3f));

        }

        private void SetUpSplit()
        {

            var split = new TwoPaneSplitView(1, 300, TwoPaneSplitViewOrientation.Horizontal);
            split.contentContainer.Add(graphView);
            split.contentContainer.Add(inspactor);
            graphRoot.Add(split);

        }

        protected override void OnEnable()
        {
            
            base.OnEnable();
            AddToolBar();

            graphRoot = new VisualElement();
            graphRoot.style.flexGrow = 1;
            rootVisualElement.Add(graphRoot);

            SetUpGraph();
            SetUpInspacter();
            SetUpSplit();

        }


        #region 그래프 관련

        /// <summary>
        /// 그래프뷰
        /// </summary>
        internal class FAED_BehaviorTreeGraphView : FAED_BaseGraphView
        {



        }

        /// <summary>
        /// 그래프 노드 생성창
        /// </summary>
        internal class FAED_BehaviorTreeSearchWindow : ScriptableObject, ISearchWindowProvider
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

        #endregion

        #region Node 관련

        internal class FAED_BehaviorTreeBaseNode : FAED_BaseNode
        {

            public Type classType;

            internal FAED_BehaviorTreeBaseNode(Type classType) : base(AssetDatabase.GetAssetPath(Resources.Load<VisualTreeAsset>("BehaviorNode")))
            {

                this.classType = classType;
                title = classType.Name;

                styleSheets.Clear();
                styleSheets.Add(Resources.Load<StyleSheet>("BehaviorNodeStyle"));

                RefreshAll();

            }

        }

        #endregion

        #region 잡것들

        /// <summary>
        /// uxml작성할줄 몰라서 만든 윈도우
        /// </summary>
        internal class FAED_VisualWindow : VisualElement
        {

            public VisualElement titleContainer { get; protected set; }
            public Label titleLabel { get; protected set; }
            public IMGUIContainer guiContainer { get; protected set; }

            public FAED_VisualWindow(string text, Position position, Color backGroundColor)
            {

                style.backgroundColor = backGroundColor;
                style.position = position;
                style.flexGrow = 1;

                CreateTitleContainer();

                titleContainer = new VisualElement();
                titleContainer.style.position = Position.Relative;
                titleContainer.style.backgroundColor = Color.black;
                titleContainer.style.flexShrink = 0;

                titleLabel = new Label(text);
                titleLabel.style.position = Position.Relative;
                titleLabel.style.fontSize = 24;

                titleContainer.Add(titleLabel);

                Add(titleContainer);


            }

            private void CreateTitleContainer()
            {

                titleContainer = new VisualElement();
                titleContainer.style.position = Position.Relative;
                titleContainer.style.backgroundColor = Color.black;

                Add(titleContainer);

            }

        }
        #endregion

    }

}