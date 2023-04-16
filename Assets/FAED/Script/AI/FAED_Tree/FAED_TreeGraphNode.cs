namespace FD.AI.Tree.Nodes
{

#if UNITY_EDITOR
    public class FAED_TreeGraphNode : UnityEditor.Experimental.GraphView.Node
    {

        public string GUID;
        public string text;
        public FAED_TreeNodeType type;

        public FAED_TreeGraphNode(string GUID, string text, FAED_TreeNodeType type)
        {

            this.GUID = GUID;
            this.text = text;
            this.type = type;

        }
    }

#endif

    public enum FAED_TreeNodeType
    {

        Root,
        Sequence,
        Tree,
        If,

    }

}
