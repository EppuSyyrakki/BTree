using BTree.Agent;
using UnityEngine;
using XNode;

namespace BTree
{
	/// <summary>
	/// Represents an actionable node in the tree graph. Can deliver an action class on request.
	/// Has no children / input ports.
	/// </summary>
	[NodeTint(0.15f, 0.175f, 0.15f)]
	public class Leaf : TreeNode
	{
		[SerializeField]
		protected float maxDuration = 10f;

		protected NodeResult CurrentResult;

		public AgentState GetState(BTreeAgent agent)
		{

		}

		public override void ResetNode()
		{
			base.ResetNode();
			CurrentResult = new NodeResult(this, Result.Running);
		}

		public override void Setup(SceneTree t)
		{
			ResetNode();

			if (Tree == null) { Tree = t; }
		}
		
		public override object GetValue(NodePort port)
		{
			if (CurrentResult != null) { graphResult = CurrentResult.Value; }
			
			return CurrentResult;
		}

		public void SetCurrentResult(Result result)
		{
			CurrentResult.Value = result;
		}
	}
}