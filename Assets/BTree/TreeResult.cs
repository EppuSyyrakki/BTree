using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BTree
{
	/// <summary>
	/// Wrapper to make node results nullable and transport the origin leaf through the tree.
	/// </summary>
	[Serializable]
	public class TreeResult : object
	{
		public ILeaf Origin { get; private set; }
		public List<Condition> Conditions { get; private set; }
		public Result Value { get; set; }
		
		public TreeResult(ILeaf origin, Result result)
		{
			Origin = origin;
			Value = result;
			Conditions = new List<Condition>();
		}

		public bool CheckConditions()
		{
			foreach (var cond in Conditions)
			{
				var port = cond.GetOutputPort(cond.OutputName);
				var result = cond.GetValue(port) as ConditionResult;

				if (result.Value == Result.Failure)
				{
					cond.Host.RecursiveFail();
					return false;
				}
			}

			return true;
		}
	}

	/// <summary>
	/// Wrapper for returning an object from a condition node.
	/// </summary>
    [Serializable]
    public class ConditionResult : object
    {
        public Result Value { get; private set; }

        public ConditionResult(Result result)
        {
            Assert.IsTrue(result != Result.Running, "Condition must never return Result.Running!");
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