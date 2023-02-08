﻿using UnityEngine;
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

		[SerializeField, Tooltip("Try to get a context with this name when entering this node.")]
		private string inContext;

		[SerializeField, Tooltip("Try to add a context with this name when exiting this node.")]
		private string outContext;

		[SerializeField, Tooltip("If the Out Context already exists, overwrite it.")]
		private bool overwriteOut = true;

        private float elapsed = 0;		

        public TreeResponse Response { get; set; }
        protected T Context { get; set; }

        protected sealed override void Setup(TreeAgent agent)
        {
            Response = new TreeResponse(this);
            base.Setup(agent);
            OnSetup();
        }

        /// <summary>
        /// Called after the base class has set up itself. Base class handles setting up Response and Agent property
        /// before this is called.
        /// </summary>
        protected abstract void OnSetup();

		public void Enter()
		{
            elapsed = 0;
            Response.Result = Result.Running;

            if (!string.IsNullOrEmpty(inContext))
            {
                if (Agent.TryGetContext(inContext, out ITreeContext context))
                {
                    this.Context = context as T;
                }
                else
                {
                    if (Agent.debugTree)
                    {
                        Debug.LogWarning($"{Agent}.{this} could not find inContext {inContext}.");
                    }

                    Response.Result = Result.Failure;
                }
            }

			OnEnter();
        }

        /// <summary>
        /// Called by the base class after getting the context from the tree with the In Context key, so the context
        /// can be used here.
        /// </summary>
		protected abstract void OnEnter();

        /// <summary>
        /// Override this to create actionable nodes that get sent to the agent after the tree is evaluated and the path
        /// to this node returns a Result.Running within its TreeResponse. Base implementation only advances the timer 
        /// and fails if it is > maxDuration.
        /// </summary>
        public virtual void Execute()
        {
            if (maxDuration < 0) { return; }

            elapsed += Time.deltaTime;

            if (elapsed > maxDuration)
            {
                Response.Result = Result.Failure;
            }
        }

        public void Exit()
		{
            OnExit();

            if (!string.IsNullOrEmpty(outContext)) 
            {
                if (Context == null)
                {
                    if (Agent.debugTree)
                    {
                        Debug.LogWarning($"{Agent}.{this} outContext {outContext} not set.");
                    }
                    
                    return;
                }

                if (!Agent.TryAddContext(outContext, Context, overwriteOut))
                {
                    if (Agent.debugTree)
                    {
                        Debug.LogWarning($"{Agent}.{this} outContext {outContext} already exists.");
                    }                   
                }                
            } 
        }

        /// <summary>
        /// Called by the base class after setting the context to the tree with the Out Context key, so any Out Context
        /// should be set before this method. This enables any triggers 
        /// </summary>
        protected abstract void OnExit();

        internal sealed override void ResetNode()
		{
            Response.Result = Result.Running;
            Context = null;
            elapsed = 0;
            OnReset();
		}

        /// <summary>
        /// Called after the base class receives a Reset() call. Base class handles resetting Response and nulling
        /// context.
        /// </summary>
        protected abstract void OnReset();

        public void Fail()
        {
            Response.Result = Result.Failure;           
            Context = null;
            Exit();
            OnFail();
        }

        /// <summary>
        /// Called after the base class receives a Fail() call. Base class handles failing Response and nulling
        /// context.
        /// </summary>
        protected abstract void OnFail();
		
		/// <summary>
		/// Override this method to create checks that need to return a value while the tree is Evaluated.
        /// Returning Success/Failure will make the tree skip Enter/Execute/Exit methods when it's being run.
		/// </summary>
		public sealed override object GetValue(NodePort port)
		{	
			return Response;
		}
	}
}