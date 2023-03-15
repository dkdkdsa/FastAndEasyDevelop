using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FD.AI.Tree.Program;
using FD.AI.Tree;
using System;

namespace FD.AI
{

    public class FAED_TreeAI : MonoBehaviour
    {

        [SerializeField] private FAED_TreeData data;
        [SerializeField, HideInInspector] private FAED_TreeAIMain main;


        [HideInInspector] public UnityEvent updateEvent;


        private void Awake()
        {
            
            main.StartAI();

        }

        private void Update()
        {

            updateEvent?.Invoke();

        }

        public void Setting()
        {

            if(transform.Find("Root") != null) 
            {

                DestroyImmediate(transform.Find("Root").gameObject);

            }
            main = new FAED_TreeAIMain(data, this);

        }

        public void ResetAI()
        {

            DestroyImmediate(transform.Find("Root").gameObject);
            data = null;
            main = null;

        }
    }

}
