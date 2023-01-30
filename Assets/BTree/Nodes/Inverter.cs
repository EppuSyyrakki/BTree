using UnityEngine;
using XNode;

namespace BTree
{
	public class Inverter : TreeNode
	{
        [SerializeField, Input(dynamicPortList: false, connectionType = ConnectionType.Override)]
        protected TreeResult input;

        public override object GetValue(NodePort port)
		{
			var result = GetResult();

			if (result == null)
			{
				return null;
			}

			if (result.Value == Result.Failure)
			{
				result.Value = Result.Success;
			}
			else if (result.Value == Result.Success)
			{
				result.Value = Result.Failure;
			}

			return result;
		}

		internal override void ResetNode()
		{			
		}
	}
}