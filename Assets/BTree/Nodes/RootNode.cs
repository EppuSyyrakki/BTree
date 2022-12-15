using UnityEngine;
using XNode;

namespace BTree
{
	/// <summary>
	/// The starting node of the behaviour tree. This is the first one accessed by the graph when evaluating.
	/// </summary>
	public class RootNode : TreeNode
	{
		[SerializeField, Input(connectionType = ConnectionType.Override)]
		public TreeResult input;

		public override object GetValue(NodePort port)
		{
			TreeResult result = GetResult();

			if (result == null) { return null; }

			return result;
		}

		internal override void ResetNode()
		{
		}
	}
}