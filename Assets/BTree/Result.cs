using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BTree
{
	/// <summary>
	/// Wrapper to make node results nullable and transport the origin leaf through the tree.
	/// </summary>
	[System.Serializable]
	public class NodeResult : object
	{
		public TreeNode Origin
		{
			get => Path[0];
			set => Path[0] = value;
		}

		public Leaf OriginAsLeaf
		{
			get
			{
				Assert.IsTrue(Origin is Leaf);
				return Path[0] as Leaf;
			}
		}

		public Result Value { get; set; }
		public List<TreeNode> Path { get; private set; }
		
		public NodeResult(TreeNode origin, Result result)
		{
			Path = new List<TreeNode>();
			Path.Add(origin);
			Value = result;
		}
	}

	public enum Result
	{
		Running,
		Success,
		Failure,
	}
}