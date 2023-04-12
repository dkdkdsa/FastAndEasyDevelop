using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace FD.System.Editor.Graph
{

    public class FAED_GraphBaseWindow<T> : EditorWindow where T : EditorWindow
    {

        public static T window;

        public List<FAED_GraphNode> nodeContainer 
        {
            
            get { return rootVisualElement.Query<FAED_GraphNode>().ToList(); }

        }

        public static void CreateGraphWindow()
        {

            window = (T)GetWindow(typeof(T));

        }

        public void Add(VisualElement element)
        {

            rootVisualElement.Add(element);

        }

    }

}