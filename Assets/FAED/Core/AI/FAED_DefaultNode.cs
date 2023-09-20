using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FD.Dev.AI
{

    public enum FAED_NodeState
    {

        Success,
        Failure,
        Running

    } 

    public abstract class FAED_Node : ScriptableObject
    {

        [HideInInspector] public GUID guid;
        public Vector2 editorPos;

        public abstract FAED_NodeState Execute();

    }

    public abstract class FAED_ActionNode : FAED_Node { }

    public abstract class FAED_CompositeNode : FAED_Node
    {

        public List<FAED_Node> childrens = new List<FAED_Node>();

    }

    public abstract class FAED_DecoratorNode : FAED_Node 
    {

        public FAED_Node children;

    }

}