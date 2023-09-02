using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FD.Core.Editors;
using UnityEditor;
using UnityEngine.UIElements;

public class TestParsing : FAED_GoogleFormParser
{

    [MenuItem("TEST/Parsing")]
    private static void Open()
    {

        var window = GetWindow<TestParsing>();
        window.Show();

    }

    private void CreateEnement()
    {

        var fDdocu = new TextField("DocumentID");
        var gidFD = new TextField("gid");
        var btn = new Button(() =>
        {

            GetFromData(fDdocu.value, gidFD.value, CreateSO);

        });

        btn.text = "CreateSO";

        rootVisualElement.Add(fDdocu); 
        rootVisualElement.Add(gidFD);
        rootVisualElement.Add(btn);

    }

    private void OnEnable()
    {
        
        CreateEnement();

    }

    private void CreateSO(bool complete, string value)
    {

        if (!complete) return;

        string[] s1 = value.Split('\n');

        for(int i = 1; i <  s1.Length; i++)
        {

            string[] s = s1[i].Split('\t');

            TestSO sp = CreateInstance<TestSO>();
            sp.enemyName = s[0];
            sp.hp = float.Parse(s[1]);

            AssetDatabase.CreateAsset(sp, $"Assets/Resources/{s[0]}.Asset");

        }

    }

}
