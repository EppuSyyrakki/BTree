using XNode;

namespace BTree
{
	/// <summary>
	/// The starting node of the behaviour tree. This is the first one accessed by the graph when evaluating.
	/// </summary>
	public class RootNode : TreeNode
	{
		[Input(connectionType = ConnectionType.Override)]
		public TreeResult childPort;

		public override object GetValue(NodePort port)
		{
			var result = GetResult();

			if (result == null) { return null; }

			graphResult = result.Value;
			return result;
		}
	}
}