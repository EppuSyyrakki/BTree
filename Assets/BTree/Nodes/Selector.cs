using UnityEngine;
using XNode;

namespace BTree
{
	/// <summary>
	/// Chooses a child that is able to run and returns its result. If all children fail, returns failure.
	/// </summary>
	public class Selector : TreeNode
	{
        [SerializeField, Input(dynamicPortList: true, connectionType = ConnectionType.Override)]
        protected TreeResult input;

        private int index;

		private bool HasNextChild => index + 1 < children.Length;

        internal override void ResetNode()
		{
			index = 0;
		}

		public override object GetValue(NodePort port)
		{
            index = 0;
            var result = GetChildResultAtIndex(index);

            if (result == null)
            {
                // Debug.LogWarning(GetType() + " received a null value");
                return null;
            }

            return Resolve(result);
        }

		private TreeResult Resolve(TreeResult result)
		{
            if (result.Value == Result.Running)
            {
                // If child is running, send it on as is.
                return result;
            }

            if (result.Value == Result.Failure)
            {
                if (HasNextChild)
                {
                    // Child failed but there is more children to go through so check them.
                    index++;
                    result = GetChildResultAtIndex(index);
                    return Resolve(result);
                }

                // This is the last child, set success.
                result.Value = Result.Failure;
            }

            return result;
        }
	}
}