using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FD.System.Editor.Graph
{

    public class FAED_GraphObject : VisualElement
    {

        public Guid guid;

        public void NewGUID()
        {

            guid = Guid.NewGuid();

        }

    }


}