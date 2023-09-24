using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
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
        [HideInInspector] public Rect editorPos;

        protected FAED_BehaviorTreeRunner runner;
        protected FAED_NodeState state;
        protected bool started;

        public FAED_NodeState Execute()
        {

            if (!started)
            {

                Enable();
                started = true;

            }

            state = OnExecute();

            if (state == FAED_NodeState.Failure || state == FAED_NodeState.Success)
            {

                Disable();
                started = false;

            }

            return state;

        }

        public virtual FAED_Node Copy(FAED_BehaviorTreeRunner runner)
        {

            this.runner = runner;

            return Instantiate(this);

        }

        public void Breaking()
        {

            Disable();
            started = false;

        }

        protected virtual void Enable() { }
        protected virtual void Disable() { }
        protected abstract FAED_NodeState OnExecute();


    }

    public abstract class FAED_ActionNode : FAED_Node { }

    public abstract class FAED_CompositeNode : FAED_Node
    {

        [HideInInspector] public List<FAED_Node> childrens = new List<FAED_Node>();

        public override FAED_Node Copy(FAED_BehaviorTreeRunner runner)
        {

            var node = Instantiate(this);

            node.childrens = childrens.ConvertAll(x => x.Copy(runner));

            return node;

        }

    }

    public abstract class FAED_DecoratorNode : FAED_Node 
    {

        [HideInInspector] public FAED_Node children;

        public override FAED_Node Copy(FAED_BehaviorTreeRunner runner)
        {

            this.runner = runner;

            var node = Instantiate(this);
            node.children = Instantiate(children);
            
            return node;

        }

    }

}