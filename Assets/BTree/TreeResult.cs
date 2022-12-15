using System;
using UnityEngine.Assertions;

namespace BTree
{
	/// <summary>
	/// Wrapper to make node results nullable and transport the origin leaf through the tree.
	/// </summary>
	[Serializable]
	public class TreeResult : object
	{
		private TreeNode origin;

		public ILeaf Origin
		{
			get
			{
				Assert.IsTrue(origin is ILeaf, $"{origin} is an end node but is not a ILeaf");
				return origin as ILeaf;
			}
		}

		public Result Value { get; set; }
		
		public TreeResult(TreeNode origin, Result result)
		{
			this.origin = origin;
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