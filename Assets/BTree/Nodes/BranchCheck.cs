using System.Collections;
using System.Linq;
using UnityEngine;
using XNode;

namespace BTree.Nodes
{
	/// <summary>
	/// Returns running if the memory specified in the enum is greater than 0.
	/// </summary>
	public class BranchCheck : Branch
	{
		[SerializeField]
		private BlackboardVariable blackboardVariable;

		[SerializeField]
		private int amount;

		public override object GetValue(NodePort port)
		{
			// Check to prevent editor errors
			if (Tree == null) { return null; }

			NodeResult result = DynamicInputs.Any() ? GetFirstChildResult() : new NodeResult(this, Result.Success);
			
			if (Tree.Agent.Blackboard.GetValue(blackboardVariable) < 1)
			{
				result.Value = Result.Failure;
			}
			
			graphResult = result.Value;
			result.Path.Add(this);
			return result;
		}
	}
}