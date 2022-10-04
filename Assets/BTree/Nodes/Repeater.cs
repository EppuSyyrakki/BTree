using UnityEngine;
using XNode;

namespace BTree.Nodes
{
	/// <summary>
	/// Repeats a single child a given number of times. If the child returns failure at any point, returns failure.
	/// </summary>
	public class Repeater : Branch
	{
		[SerializeField, Tooltip("0 <= repeats indefinitely")]
		private int repeat;

		private int _counter;

		public override void ResetNode()
		{
			base.ResetNode();
			_counter = 0;
		}

		public override object GetValue(NodePort port)
		{
			var result = GetFirstChildResult();

			if (result == null)
			{
				Debug.LogWarning(GetType() + " received a null value");
				return null;
			}

			if (result.Value == Result.Success)
			{
				if (repeat <= 0 || _counter < repeat)
				{
					_counter++;
					result.Value = Result.Running;
					result.Path.Add(this);
					RecursiveResetChildren();
					return result;
				}
			}

			result.Path.Add(this);
			return result;
		}

	}
}