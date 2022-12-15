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
		private bool HasNextChild => _index + 1 < children.Length;

		internal override void ResetNode()
		{
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
				return result;
			}

			if (result.Value == Result.Success)
			{
				if (HasNextChild)
				{
					// Child succeeded but there is more children to go through so return running.
					_index++;
					result = GetChildResultAtIndex(_index);
					return result;
				}

				// This is the last child, set success.
				result.Value = Result.Success;
			}

			return result;
		}
	}
}