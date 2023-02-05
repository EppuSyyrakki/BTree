using System;
using System.Collections.Generic;

namespace BTree
{
	/// <summary>
	/// Wrapper to make node results nullable and transport the origin leaf and possible conditions through the tree.
	/// </summary>
	[Serializable]
	public class TreeResponse : object
	{
		public ILeaf Origin { get; private set; }
		public List<Condition> Conditions { get; private set; }
		public Result Result { get; internal set; }
		
		public TreeResponse(ILeaf origin)
		{
			Origin = origin;
			Conditions = new List<Condition>();
		}

		public TreeResponse(Result result)
		{
			Result = result;
		}

		public bool CheckConditions()
		{
			foreach (var cond in Conditions)
			{
				if (cond.Check()) { continue; }

				Result = Result.Failure;
                cond.Host.RecursiveFail();
                return false;
            }

			return true;
		}
	}

    public enum Result
	{
		Running,
		Success,
		Failure,
	}
}