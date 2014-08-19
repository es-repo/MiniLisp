namespace MiniLisp.Trees
{
    public class TreeNodeInfo<T>
    {
        public TreeNode<T> Node { get; private set; }

        public TreeNodeInfo<T> ParentNodeInfo { get; private set; }

        public int IndexAmongSiblings { get; private set; }

        public int Depth
        {
            get;
            private set;
        }

        public TreeNodeInfo(TreeNode<T> node, TreeNodeInfo<T> parentNodeInfo, int indexAmongSiblings, int depth)
        {
            Node = node;
            ParentNodeInfo = parentNodeInfo;
            IndexAmongSiblings = indexAmongSiblings;
            Depth = depth;
        }
    }
}
