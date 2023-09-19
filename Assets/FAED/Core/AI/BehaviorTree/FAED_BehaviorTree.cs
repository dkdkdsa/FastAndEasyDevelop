using FD.Dev.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FAED_BehaviorTree : ScriptableObject
{

    [HideInInspector] public bool isMemory;

    public FAED_Node rootNode;
    public List<FAED_Node> nodes = new List<FAED_Node>();

}
