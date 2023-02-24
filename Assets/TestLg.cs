using FD.AI.Tree.Program;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLg : FAED_TreeActionNode
{

    [SerializeField] private string lg;
    [SerializeField] private FAED_TreeNodeState state = FAED_TreeNodeState.Success;

    public override void Behavior()
    {

        Debug.Log(lg);
        Complete(state);

    }

}
