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
		private T context;

		protected TreeAgent Agent { get; private set; }
        public Result Result { get; set; }
        protected T Context
        {
            get
            {
                if (context is NoContext)
                {
                    Debug.LogError($"{tree.agent.gameObject}.{this} trying to use context while having NoContext type");
                    return null;
                }

                return context;
            }
            set => context = value;
        }

        /// <summary>
        /// Override this to create actionable nodes that get sent to the agent after the tree is evaluated. 
        /// The base implementation only advances the timer.
        /// </summary>
        public virtual void Execute()
        {
            if (maxDuration < 0) { return; }

            elapsed += Time.deltaTime;

            if (elapsed > maxDuration)
            {
                Result = Result.Failure;
            }
        }

		public void Enter(TreeAgent agent)
		{
            elapsed = 0;
            Agent = agent;
            Result = Result.Running;

            if (!string.IsNullOrEmpty(inContext))
            {
                if (tree.TryGetContext(inContext, out ITreeContext context))
                {
                    this.context = context as T;
                }
                else
                {
                    if (tree.debugTree)
                    {
                        Debug.LogWarning($"{agent.gameObject}.{this} could not find InputVariable {inContext}.");
                    }
                    
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
                    if (tree.debugTree)
                    {
                        Debug.LogWarning($"{tree.agent.gameObject.name} TreeAgent: context {outContext} not set.");
                    }
                    
                    return;
                }

                if (!tree.TryAddContext(outContext, context, overwriteOut))
                {
                    if (tree.debugTree)
                    {
                        Debug.LogWarning($"{tree.agent.gameObject.name} TreeAgent: context {outContext} already exists.");
                    }                   
                }                
            }
        }

        /// <summary>
        /// Called by the base class before setting the context to the tree with the Out Context key, so any
        /// required context can be set inside this method.
        /// </summary>
        protected abstract void OnExit();

        internal override void ResetNode()
		{
            Result = Result.Waiting;
            context = null;
            Agent = null;
            elapsed = 0;
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
			return new TreeResponse(this, Result);
		}
	}
}