using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FD.AI.Tree;
using FD.AI.UI;
using System.Linq;

namespace FD.AI.Tree.Program
{
    [System.Serializable]
    public class FAED_TreeAIMain
    {

        [SerializeField] FAED_TreeActionNode node;
        [SerializeField] private FAED_TreeRootNode rootNode;
        [SerializeField] private FAED_TreeAI aI;
        [SerializeField] private FAED_TreeData data;
        [SerializeField] private List<FAED_TreeAIUI> scriptList = new List<FAED_TreeAIUI>();

        public FAED_TreeAIMain(FAED_TreeData data, FAED_TreeAI aI)
        {

            this.data = data;
            this.aI = aI;

            foreach(var item in data.nodes)
            {

                GameObject obj = new GameObject(item.text);

                foreach(var port in item.ports)
                {

                    GameObject portObj = new GameObject(port);

                    portObj.transform.SetParent(obj.transform);

                }

                if(item.type == Node.FAED_TreeNodeType.Root)
                {

                    rootNode = obj.AddComponent<FAED_TreeRootNode>();
                    

                }
                else if(item.type == Node.FAED_TreeNodeType.Sequence)
                {

                    obj.AddComponent<FAED_Sequence>();

                }
                else if(item.type == Node.FAED_TreeNodeType.If)
                {

                    obj.name = $"{obj.name}(If)";

                }
                else if (item.type == Node.FAED_TreeNodeType.Tree)
                {

                    obj.name = $"{obj.name}(Tree)";

                }

                obj.transform.SetParent(aI.transform);

                scriptList.Add(new FAED_TreeAIUI { obj = obj, GUID = item.GUID });

            }

            foreach(var item in data.links)
            {

                GameObject baseObj = scriptList.Find(x => x.GUID == item.baseGUID).obj;
                GameObject cObj = scriptList.Find(x => x.GUID == item.targetGUID).obj;

                cObj.transform.SetParent(baseObj.transform.Find(item.portName));

                scriptList.Find(x => x.GUID == item.baseGUID).connect.Add(item.targetGUID);

            }
            
            

        }

        public void StartAI()
        {

            foreach(var item in data.nodes)
            {

                if(item.type == Node.FAED_TreeNodeType.Root)
                {

                    FAED_TreeRootNode obj = aI.transform.Find(item.text).gameObject.GetComponent<FAED_TreeRootNode>();

                    var list = data.links.FindAll(x => x.baseGUID == item.GUID).OrderBy(x => x.portName).ToList();

                    list.ForEach(x =>
                    {

                        if (x.baseGUID == item.GUID)
                        {

                            FindAllNode(obj, x.targetGUID);

                        }

                    });

                    obj.Setting(this, aI, item.GUID);

                }

                if (item.type == Node.FAED_TreeNodeType.Sequence)
                {

                    FAED_Sequence obj = scriptList.Find(x => x.GUID == item.GUID).obj.GetComponent<FAED_Sequence>();

                    var list = data.links.FindAll(x => x.baseGUID == item.GUID).OrderBy(x => x.portName).ToList();

                    list.ForEach(x =>
                    {

                        if (x.baseGUID == item.GUID)
                        {

                            FindAllNode(obj, x.targetGUID);

                        }

                    });

                    obj.Setting(this, aI, item.GUID);

                }

                if (item.type == Node.FAED_TreeNodeType.If)
                {

                    FAED_TreeBoolNode obj = scriptList.Find(x => x.GUID == item.GUID).obj.GetComponent<FAED_TreeBoolNode>();
                    data.links.ForEach(x =>
                    {

                        if (x.baseGUID == item.GUID)
                        {

                            FAED_SettingTree cntStr = scriptList.Find(y => y.GUID == x.targetGUID).obj.GetComponent<FAED_SettingTree>();
                            //cntStr.Setting(this, aI, x.targetGUID);
                            
                            if(x.portName == "NextTrue")
                            {

                                //obj.trueAction.Add(cntStr);
                                //cntStr.SettingRootNode(obj);

                                FindAllNode(obj, x.targetGUID, ref obj.trueAction);

                            }
                            else
                            {

                                //obj.falseAction.Add(cntStr);
                                //cntStr.SettingRootNode(obj);

                                FindAllNode(obj, x.targetGUID, ref obj.falseAction);

                            }

                        }

                    });

                    obj.Setting(this, aI, item.GUID);

                }

            }

            rootNode.Execute();

        }

        private void FindAllNode<T>(T root, string guid) where T : FAED_TreeAINode, IFAED_StateTreeNode<FAED_TreeAINode>
        {

            var cntStr = scriptList.Find(x => x.GUID == guid).obj.GetComponent<FAED_SettingTree>();
            root.nodeActions.Add(cntStr);
            cntStr.Setting(this, aI, guid);
            cntStr.SettingRootNode(root);

            if (data.links.Find(x => x.baseGUID == guid) != null)
            {

                if (data.nodes.Find(x => x.GUID == guid).type == Node.FAED_TreeNodeType.Sequence) return;
                if (data.nodes.Find(x => x.GUID == guid).type == Node.FAED_TreeNodeType.If) return;

                FindAllNode(root, data.links.Find(x => x.baseGUID == guid).targetGUID);

            }

        }

        private void FindAllNode<T>(T root, string guid, ref List<FAED_TreeAINode> list) where T : FAED_TreeAINode, IFAED_StateTreeNode<FAED_TreeAINode>
        {

            var cntStr = scriptList.Find(x => x.GUID == guid).obj.GetComponent<FAED_SettingTree>();
            list.Add(cntStr);
            cntStr.Setting(this, aI, guid);
            cntStr.SettingRootNode(root);

            if (data.links.Find(x => x.baseGUID == guid) != null)
            {

                if (data.nodes.Find(x => x.GUID == guid).type == Node.FAED_TreeNodeType.Sequence) return;
                if (data.nodes.Find(x => x.GUID == guid).type == Node.FAED_TreeNodeType.If) return;

                FindAllNode(root, data.links.Find(x => x.baseGUID == guid).targetGUID, ref list);

            }

        }

    }

}
