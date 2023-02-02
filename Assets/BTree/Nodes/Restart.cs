namespace BTree
{
    public class Restart : TreeNode, ILeaf
    {
        public Result Result => Result.Waiting;
        public bool IsInitialized => true;

        public void Enter(TreeAgent agent) { }

        public void Execute() 
        { 
            tree.Restart(); 
        }

        public void Exit() { }

        public void Fail() { }

        internal override void ResetNode() { }
    }
}