using UnityEngine;
using XNode;

namespace BTree
{
    /// <summary>
    /// A check that is performed when the tree is evaluated. Inherit from this class and implement checking
    /// the condition by overriding object GetValue(NodePort port) and returning a Result. A Condition should
    /// never return a Result.Running.
    /// </summary>
    [NodeTint(0.15f, 0.25f, 0.25f)]
    public abstract class Condition : TreeNode
    {
        [SerializeField]
        protected bool invert;

        internal Parallel Host { get; set; }
        
        internal bool Check()
        {
            bool result = OnCheck();
            return invert ? !result : result;
        }

        protected abstract bool OnCheck();

        public override object GetValue(NodePort port)
        {
            return new TreeResponse(Check() ? Result.Success : Result.Failure); 
        }  
    }
}