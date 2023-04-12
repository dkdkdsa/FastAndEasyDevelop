using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FD.System.Editor.Graph
{

    public class FAED_GraphNode : FAED_GraphObject
    {
        
        public FAED_GraphNode()
        {

            style.position = Position.Absolute;
            
            style.borderBottomLeftRadius = 10;
            style.borderBottomRightRadius = 10;
            style.borderTopLeftRadius = 10;
            style.borderTopRightRadius = 10;

            style.width = 150;
            style.height = 75;

            style.backgroundColor = new StyleColor(new Color32(87, 87, 87, 255));

            RegisterCallback<DragUpdatedEvent>(evt =>
            {

                Debug.Log("123");
                style.left = evt.localMousePosition.x;
                style.top = evt.localMousePosition.y;

            });

        }

    }

}