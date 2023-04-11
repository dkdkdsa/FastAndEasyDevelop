using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FD.System.Editor.Graph
{

    public class FAED_GraphBaseWindow<T> : EditorWindow where T : EditorWindow
    {

        public static T window;

        public static void CreateGraphWindow()
        {

            window = (T)GetWindow(typeof(T));

        }

    }

}