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
		
		public TreeResponse(ILeaf origin, Result result)
		{
			Origin = origin;
			Result = result;
			Conditions = new List<Condition>();
		}

		public bool CheckConditions()
		{
			foreach (var cond in Conditions)
			{
				if (cond.Check()) { continue; }

                cond.Host.RecursiveFail();
                return false;
            }

			return true;
		}
	}

    public enum Result
	{
		Waiting,
		Running,
		Success,
		Failure,
	}
}