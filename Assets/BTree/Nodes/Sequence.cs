using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BTree.Nodes
{
	/// <summary>
	/// Loops through child nodes. If any child fails, returns failure. Returns success after all nodes
	/// succeeded and has repeated given times.
	/// </summary>
	public class Sequence : Branch
	{
		private int _index;
		private List<TreeNode> _succeededChildren = new List<TreeNode>();
		private bool HasNextChild => _index + 1 < children.Length;

		public override void ResetNode()
		{
			base.ResetNode();
			_index = 0;
		}

		public override object GetValue(NodePort port)
		{
			var result = GetChildResultAtIndex(_index);

			if (result == null)
			{
				Debug.LogWarning(GetType() + " received a null value");
				return null;
			}

			return Resolve(result);
		}

		private TreeResult Resolve(TreeResult result)
		{
			if (result.Value == Result.Running)
			{
				// If child is running, send it on as is.
				graphResult = result.Value;
				result.Path.Add(this);
				return result;
			}

			if (result.Value == Result.Success)
			{
				if (HasNextChild)
				{
					// Child succeeded but there is more children to go through so return running.
					_succeededChildren.Add(children[_index]);
					_index++;
					result = GetChildResultAtIndex(_index);
					result.Path.Add(this);
					graphResult = result.Value;
					return result;
				}

				// This is the last child, set success.
				result.Path.Add(this);
				result.Value = Result.Success;
			}

			graphResult = result.Value;
			return result;
		}
	}
}