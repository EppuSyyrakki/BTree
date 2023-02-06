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
		
		internal TreeResponse(ILeaf origin) : this() { Origin = origin; }
		internal TreeResponse(Result result) : this() { Result = result; }
		private TreeResponse() { Conditions = new List<Condition>(); }

		public bool CheckConditions()
		{
			foreach (var c in Conditions)
			{
				if (c.Check()) { continue; }

				Result = Result.Failure;
                c.Host.RecursiveFail();
                return false;
            }

			return true;
		}
	}

    public enum Result
	{
		Running = 0,
		Success,
		Failure,
	}
}