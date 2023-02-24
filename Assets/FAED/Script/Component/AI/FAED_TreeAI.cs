using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FD.AI.Tree.Program;
using FD.AI.Tree;

namespace FD.AI
{

    public class FAED_TreeAI : MonoBehaviour
    {

        [SerializeField] private FAED_TreeData data;
        [SerializeField] private FAED_TreeAIMain main;


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

            for(int i = 0; i < transform.childCount; i++)
            {

                DestroyImmediate(transform.GetChild(i).gameObject);

            }

            main = new FAED_TreeAIMain(data, this);

        }

    }

}
