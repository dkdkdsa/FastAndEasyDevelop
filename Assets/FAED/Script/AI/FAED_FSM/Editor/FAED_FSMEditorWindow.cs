#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FD.UI.Tool;
using UnityEditor;

namespace FD.AI.FSM.Window
{

    public class FAED_FSMEditorWindow : FAED_GraphViewTool
    {

        [MenuItem("FAED_AI/FAED_FSM")]
        private static void ShowWindow()
        {

            var window = CreateWindow<FAED_FSMEditorWindow>();
            window.Show();

        }

        private void OnEnable()
        {

            CreateWindowElement("FAED_FSM", true, true);
            SetToolBar();

        }

        private void SetToolBar()
        {

            #region 파일 이름 입력하는곳

            toolbar.AddTextField("FileName :");
            toolbar.AddButton(() => { }).text = "SaveFile";
            toolbar.AddButton(() => { }).text = "LoadFile";

            #endregion

        }

    }

}

#endif