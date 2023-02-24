using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FD.AI.Tree.Program;

public class TestB : FAED_TreeBoolNode
{

    [SerializeField] private bool asdf;

    public override bool Comparison()
    {

        return asdf;

    }
}
