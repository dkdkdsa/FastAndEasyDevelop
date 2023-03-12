using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FD.Dev;
using FD.AI;
using System;
using UnityEngine.Events;

public enum SS
{

    A,
    B,
    C,
    D,
    E,
    F,
    G,
    H,
    I,
    J,
              
}

[ASDFFF("asdf")]
public class Test : MonoBehaviour
{

    [SerializeField] private FAED_AI aI;
    [SerializeField] private SpriteRenderer sprite;

    private void Awake()
    {

        Testasdf a = Resources.Load<Testasdf>("asdf");
        a.A();

    }

    private void Update()
    {

        return;

        if (Input.GetKeyDown(KeyCode.A))
        {

            aI.ChangeState("LS"); 
            aI.ChangeBoolState("L", false);

        }
        else if (Input.GetKeyDown(KeyCode.S))
        {

            aI.ChangeState("LS");
            aI.ChangeBoolState("L", true);

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {

            aI.ChangeState("RS");
            aI.ChangeBoolState("R", false);

        }
        else if (Input.GetKeyDown(KeyCode.F))
        {

            aI.ChangeState("RS");
            aI.ChangeBoolState("R", true);

        }

    }

    public void LS()
    {

        sprite.color = Color.white;

    }

    public void L()
    {

        sprite.color = Color.gray;
    }

    public void R()
    {

        sprite.color = Color.blue;
    }

    public void RS()
    {

        sprite.color = Color.red;

    }

}
