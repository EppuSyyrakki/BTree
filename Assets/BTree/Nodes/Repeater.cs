using UnityEngine;
using XNode;

namespace BTree
{
	/// <summary>
	/// Repeats a single child a given number of times. If the child returns failure at any point, returns failure.
	/// </summary>
	public class Repeater : TreeNode
	{
        [SerializeField, Input(dynamicPortList: false, connectionType = ConnectionType.Override)]
        protected TreeResult input;

        [SerializeField, Tooltip("0 <= repeats indefinitely")]
		private int repeat;

		private int _counter;

		internal override void ResetNode()
		{
			_counter = 0;
		}

		public override object GetValue(NodePort port)
		{
			var result = GetResult();

			if (result == null)
			{
				Debug.LogWarning(GetType() + " received a null value");
				return null;
			}

			if (result.Value != Result.Running)
			{
				if (repeat <= 0 || _counter < repeat)
				{
					_counter++;
					result.Value = Result.Running;
					RecursiveResetChildren();
					return result;
				}
			}

			return result;
		}
	}
}