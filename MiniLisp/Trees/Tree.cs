using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniLisp.Trees
{
    public class Tree<T>
    {
        #region Fields

        private TreeNodeInfo<T>[] _traversedDepthFirstPostOrder;
        
        #endregion

        #region Properties

        public TreeNode<T> Root { get; private set; }

        #endregion

        #region Constructors

        public Tree(TreeNode<T> root)
        {
            Root = root;
        }

        #endregion

        #region Traversal

        public TreeNodeInfo<T>[] TraverseDepthFirstPostOrder()
        {
            return _traversedDepthFirstPostOrder ??
                (_traversedDepthFirstPostOrder = TraverseDepthFirstPostOrder(Root).ToArray());
        }

        class TraversedTreeNodeInfo
        {
            public bool IsVisited { get; set; }
            public TreeNodeInfo<T> NodeInfo { get; set; }
        }

        public static IEnumerable<TreeNodeInfo<T>> TraverseDepthFirstPostOrder(TreeNode<T> root)
        {
            TreeNodeInfo<T> rootNodeInfo = new TreeNodeInfo<T>(root, null, 0, 0);
            Stack<TraversedTreeNodeInfo> stack = new Stack<TraversedTreeNodeInfo>(new[]
                {
                    new TraversedTreeNodeInfo { NodeInfo = rootNodeInfo }
                });

            while (stack.Count > 0)
            {
                TraversedTreeNodeInfo vni = stack.Peek();
                if (vni.IsVisited)
                {
                    stack.Pop();
                    yield return vni.NodeInfo;
                }
                else
                {
                    vni.IsVisited = true;
                    int indexAmongSiblings = vni.NodeInfo.Node.Children.Count - 1;
                    foreach (TreeNode<T> n in vni.NodeInfo.Node.Children.Reverse())
                    {
                        TreeNodeInfo<T> childNodeInfo = new TreeNodeInfo<T>(n, vni.NodeInfo.Node, indexAmongSiblings, vni.NodeInfo.Depth + 1);
                        stack.Push(new TraversedTreeNodeInfo { NodeInfo = childNodeInfo });
                        indexAmongSiblings--;
                    }
                }
            }
        }

        #endregion

        public R Fold<R>(Func<TreeNodeInfo<T>, R[], R> func)
        {
            return FoldCore(TraverseDepthFirstPostOrder(), func);
        }

        public static R Fold<R>(TreeNode<T> node, Func<TreeNodeInfo<T>, R[], R> func)
        {
            IEnumerable<TreeNodeInfo<T>> traversedDepthFirstPostOrderNodes = TraverseDepthFirstPostOrder(node);
            return FoldCore(traversedDepthFirstPostOrderNodes, func);
        }

        private static R FoldCore<R>(IEnumerable<TreeNodeInfo<T>> traversedDepthFirstPostOrder, Func<TreeNodeInfo<T>, R[], R> func)
        {
            Stack<R> foldedChildrenQueue = new Stack<R>();
            foreach (TreeNodeInfo<T> nodeInfo in traversedDepthFirstPostOrder)
            {
                int childrenCount = nodeInfo.Node.Children.Count;
                R[] foldedChildren = new R[childrenCount];
                for (int i = childrenCount - 1; i >= 0; i--)
                {
                    foldedChildren[i] = foldedChildrenQueue.Pop();
                }

                R r = func(nodeInfo, foldedChildren);
                foldedChildrenQueue.Push(r);
            }

            return foldedChildrenQueue.Pop();
        }
    }
}
