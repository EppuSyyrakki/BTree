using System;
using UnityEngine;
using XNode;

namespace BTree
{
	/// <summary>
	/// Represents an actionable node that requires a context in the tree graph. Inherit any classes that make 
	/// the agent do something from this class.
	/// </summary>
	[NodeTint(0.15f, 0.175f, 0.15f)]
	public abstract class Leaf<T> : TreeNode, ILeaf where T : class, ITreeContext
	{
        [SerializeField, Tooltip("Negative value means indefinite.")]
        protected float maxDuration = -1f;

		[SerializeField, Tooltip("")]
		private string inContext;

		[SerializeField, Tooltip("")]
		private string outContext;

		[SerializeField, Tooltip("")]
		private bool overwriteOut = true;

        private float elapsed = 0;		
        private TreeResult currentResult;

		protected T context;
		protected TreeAgent agent;

        public ITreeContext Context => context;
        public float MaxDuration => maxDuration;
		public Result Result
		{
			get => currentResult.Value;
			set => currentResult.Value = value;
		}

        /// <summary>
        /// Override this to create actionable nodes that get sent to the agent after the tree is evaluated. 
        /// The base implementation only advances the timer.
        /// </summary>
        public virtual void Execute()
        {
            elapsed += Time.deltaTime;

            if (elapsed > maxDuration)
            {
                Result = Result.Failure;
            }
        }

		public void Enter(TreeAgent agent)
		{
            elapsed = 0;
            this.agent = agent;

            if (!String.IsNullOrEmpty(inContext))
            {
                if (tree.TryGetContext(inContext, out ITreeContext context))
                {
                    this.context = context as T;
                }
                else
                {
                    Debug.LogWarning($"{agent.gameObject} TreeAgent: Could not find {this} InputVariable {inContext}.");
                    Result = Result.Failure;
                }
            }

			OnEnter();
        }

        /// <summary>
        /// Called by the base class after getting the context from the tree with the In Context key, so the context
        /// can be used here.
        /// </summary>
		protected abstract void OnEnter();

		public void Exit()
		{
            OnExit();

            if (!String.IsNullOrEmpty(outContext)) 
            {
                if (context == null)
                {
                    Debug.LogWarning($"{agent.gameObject} TreeAgent: context {outContext} not set.");
                    return;
                }

                if (!tree.TryAddContext(outContext, context, overwriteOut))
                {
                    Debug.LogWarning($"{agent.gameObject} TreeAgent: variable {outContext} already exists.");
                }                
            }   
        }

        /// <summary>
        /// Called by the base class before setting the context to the tree with the Out Context key, so any
        /// required context can be set inside this method.
        /// </summary>
        protected abstract void OnExit();

        /// <summary>
        /// Make sure to reset all members here. Reset is called when the whole tree fails and the evaluation starts
        /// from a fresh state.
        /// </summary>
        protected abstract void ResetLeaf();

        internal override void ResetNode()
		{
            currentResult = new TreeResult(this, Result.Running);
            context = null;
            agent = null;
            elapsed = 0;
            ResetLeaf();
		}

		internal override void Setup(Tree t)
		{
			ResetNode();

			if (tree == null) { tree = t; }
		}

        public void Fail()
        {
            Result = Result.Failure;
            context = null;
        }
		
		/// <summary>
		/// Override this method to create checks that need to return a value while the tree is Evaluated.
        /// Returning Success/Failure will make the tree skip Enter/Execute/Exit methods when it's being run.
		/// </summary>
		public override object GetValue(NodePort port)
		{	
			return currentResult;
		}
	}
}