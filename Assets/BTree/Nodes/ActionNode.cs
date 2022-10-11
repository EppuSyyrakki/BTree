using TypeReferences;
using UnityEngine;
using XNode;

namespace BTree
{
	/// <summary>
	/// Represents an actionable node in the tree graph. Can deliver an action class on request.
	/// Has no children / input ports.
	/// </summary>
	[NodeTint(0.15f, 0.175f, 0.15f)]
	public class ActionNode : TreeNode
	{
		[SerializeField, Tooltip("Negative value means indefinite.")]
		protected float maxDuration = -1f;

        protected TreeResult CurrentResult;

		[SerializeField]
		[Inherits(typeof(AgentAction), ShortName = true), TypeOptions(ExpandAllFolders = true)]
		private TypeReference returnClass;

		public float MaxDuration => maxDuration;

		public AgentAction GetAction()
		{
			return System.Activator.CreateInstance(returnClass) as AgentAction;
        }

		public override void ResetNode()
		{
			base.ResetNode();
			CurrentResult = new TreeResult(this, Result.Running);
		}

		public override void Setup(Tree t)
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