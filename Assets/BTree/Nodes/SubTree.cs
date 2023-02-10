using UnityEngine;
using XNode;

namespace BTree
{
    public class SubTree : Branch
    {
        [SerializeField]
        private TreeAsset subTree;

        private TreeAsset tree;

        protected override void Setup(TreeAgent agent)
        {
            if (subTree == null)
            {
                Debug.LogError($"{agent} has an empty Sub Tree!");
                return;
            }

            tree = (TreeAsset)subTree.Copy();
            tree.Initialize(agent);
            base.Setup(agent);
        }

        public override object GetValue(NodePort port)
        {
            if (!initialized) { return null; }

            if (storedResponse != null) { return storedResponse; }

            var response = ResolveConditions(tree.Root.Response);
            return response;
        }

        internal override void ResetNode()
        {
            storedResponse = null;
            tree.ResetNodes();
        }
    }
}
