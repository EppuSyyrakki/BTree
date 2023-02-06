using UnityEngine;
using XNode;

namespace BTree
{
    public class SubTree : TreeNode
    {
        [SerializeField]
        private TreeAsset subTree;

        private TreeAsset tree;
        private TreeResponse storedResponse = null;

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

            var response = tree.Root.Response;

            if (response.Result == Result.Failure || response.Result == Result.Success)
            {
                storedResponse = response;
                return storedResponse;
            }

            return response;
        }

        internal override void ResetNode()
        {
            storedResponse = null;
            tree.ResetNodes();
        }
    }
}
