using System;
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
        [HideInInspector] public Vector2 editorPos;
        [HideInInspector] public Type currentObjectType;
        

        public abstract FAED_NodeState Execute();
        public virtual void Init() { }
        public virtual void Kill() { }

    }

    public abstract class FAED_ControlFlowNode : FAED_Node
    {

        public List<FAED_Node> childrens;

    }

    public abstract class FAED_DecoratorNode : FAED_Node
    {

        public FAED_Node children;

    }


}