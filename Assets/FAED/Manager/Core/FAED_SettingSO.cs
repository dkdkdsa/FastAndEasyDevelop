using FD.System.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FAED_SettingSO : ScriptableObject
{

    [HideInInspector] public FAED_PoolListSO poolList;
    [HideInInspector] public bool usePooling;

}
