using System.Linq;
using UnityEngine;
using XNode;

namespace BTree.Nodes
{
	public class Inverter : Branch
	{
		public override object GetValue(NodePort port)
		{
			var result = GetFirstChildResult();

			if (result == null)
			{
				Debug.LogWarning("Inverter received a null value from child.");
				return null;
			}

			if (result.Value == Result.Failure)
			{
				result.Value = Result.Success;
			}
			else if (result.Value == Result.Success)
			{
				result.Value = Result.Failure;
			}

			result.Path.Add(this);
			graphResult = result.Value;
			return result;
		}
	}
}