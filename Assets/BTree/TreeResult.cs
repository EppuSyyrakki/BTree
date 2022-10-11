using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BTree
{
	/// <summary>
	/// Wrapper to make node results nullable and transport the origin leaf through the tree.
	/// </summary>
	[System.Serializable]
	public class TreeResult : object
	{
		public TreeNode Origin
		{
			get => Path[0];
			set => Path[0] = value;
		}

		public ActionNode OriginAsActionNode
		{
			get
			{
				Assert.IsTrue(Origin is ActionNode);
				return Path[0] as ActionNode;
			}
		}

		public Result Value { get; set; }
		public List<TreeNode> Path { get; private set; }
		
		public TreeResult(TreeNode origin, Result result)
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