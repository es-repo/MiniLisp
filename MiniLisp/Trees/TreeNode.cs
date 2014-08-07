using System.Collections.Generic;

namespace MiniLisp.Trees
{
    public class TreeNode<T> : List<TreeNode<T>>
    {
        public T Value { get; private set; }

        public IList<TreeNode<T>> Children { get { return this; } }

        public bool IsLeaf
        {
            get { return Count == 0; }
        }

        public TreeNode(T value)
        {
            Value = value;
        }
    }
}
