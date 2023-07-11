using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace FD.Core.Editors
{

    public class FAED_BaseNode : Node
    {
        
        public Guid guid { get; protected set; }

        public FAED_BaseNode()
        {

            guid = Guid.NewGuid();

        }

        public void RefreshAll()
        {

            RefreshExpandedState();
            RefreshPorts();

        }

    }

}