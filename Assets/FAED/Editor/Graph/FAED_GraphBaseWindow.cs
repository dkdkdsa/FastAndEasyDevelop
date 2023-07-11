using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace FD.Core.Editors
{

    public abstract class FAED_GraphBaseWindow<T> : EditorWindow where T : FAED_BaseGraphView
    {

        protected T graphView;

        protected abstract void OpenGraphWindow();

    }

}