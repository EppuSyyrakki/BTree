using UnityEngine;

namespace BTree
{
	/// <summary>
	/// The starting node of the behaviour tree. This is the first one accessed by the graph when evaluating.
	/// </summary>
	public class RootNode : TreeNode
	{
		[SerializeField, Input(connectionType = ConnectionType.Override)]
		public TreeResponse input;

		public TreeResponse Response => GetChildResponse();
	}
}