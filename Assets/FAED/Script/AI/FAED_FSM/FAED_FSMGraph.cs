#if UNITY_EDITOR
using FD.UI;
using FD.UI.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace FD.AI.FSM
{

    public class FAED_FSMGraph : FAED_GraphView
    {

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            ports.ForEach(funcCall: (port) =>
            {

                if (startPort != port && startPort.node != port.node)
                {

                    compatiblePorts.Add(port);

                }

            });

            return compatiblePorts;
            
        }

    }

}
#endif
