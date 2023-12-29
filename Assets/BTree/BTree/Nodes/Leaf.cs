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

        [SerializeField, Tooltip("Try to get a context with this name when entering this node.")]
        private string inContext;

        [SerializeField, Tooltip("Try to add a context with this name when exiting this node.")]
        private string outContext;

        [SerializeField, Tooltip("If the Out Context already exists, overwrite it.")]
        private bool overwriteOut = true;

        [SerializeField, Tooltip("Fail if In Context is set to null")]
        private bool failNullCtx = false;

        private float elapsed = 0;
        private T context;

        public TreeResponse Response { get; set; }

        public event Func<bool> OnExceptionFail;

        protected T Context
        {
            get
            {
                if (context == null) { throw new ContextNullException("Context possibly destroyed while running " + this); }
                return context;
            }
            set
            {
                if (failNullCtx && value == null) { throw new ContextNullException(this + " received a null Context"); }
                context = value;
            }
        }

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
            try
            {
                if (!string.IsNullOrEmpty(inContext) && Agent.TryGetContext(inContext, out ITreeContext context))
                {
                    Context = context as T;
                }

                OnEnter();
            }
            catch (ContextNullException)
            {
                ExceptionFailure();
            }
        }

        /// <summary>
        /// Called by the base class when entering this node, after getting any In Context from the tree with the 
        /// so the context can be used inside this method.
        /// </summary>
		protected abstract void OnEnter();

        public void Execute()
        {
            try
            {
                OnExecute();
            }
            catch (ContextNullException)
            {
                ExceptionFailure();
                return;
            }

            if (maxDuration < 0) { return; }

            elapsed += Time.deltaTime;

            if (elapsed > maxDuration)
            {
                Response.Result = Result.Failure;
            }
        }

        /// <summary>
        /// Called by the base class when getting an Execute() call from the Agent. This method is called first, 
        /// then a max duration timer advanced in the base class if set to > 0 on the node.
        /// </summary>
        protected abstract void OnExecute();

        public void Exit()
        {
            try
            {
                OnExit();

                if (!string.IsNullOrEmpty(outContext) && !Agent.TryAddContext(outContext, Context, overwriteOut))
                {
                    Debug.LogWarning($"{Agent}.{this} outContext {outContext} already exists.");
                }
            }
            catch (ContextNullException)
            {
                ExceptionFailure();
            }
        }

        /// <summary>
        /// Called after the last OnExecute call by the base class, before adding Out Context to the tree - so the
        /// Context can be set inside this method.
        /// </summary>
        protected abstract void OnExit();

        internal sealed override void ResetNode()
        {
            Response.Result = Result.Running;
            context = null;
            elapsed = 0;
            OnReset();
        }

        /// <summary>
        /// Called after the base class receives a Reset() call. Base class handles resetting Response and nulling
        /// context.
        /// </summary>
        protected abstract void OnReset();

        /// <summary>
        /// Forces the Response.Result to Failure. Does NOT halt execution - if called from outside, Enter, Execute 
        /// and Exit might still run (and fail through exception, which will trigger the tree to be evaluated immediately).
        /// </summary>
        public void Fail()
        {
            Response.Result = Result.Failure;
            OnFail();
        }

        private void ExceptionFailure()
        {
            Response.Result = Result.Failure;
            bool handled = OnExceptionFail.Invoke();

            if (handled)
            {
                // Debug.LogWarning($"{Agent}.{this} exception failure handled successfully.");
            }
        }

        /// <summary>
        /// Called after the base class receives a Fail() call. Base class handles failing Response and nulling
        /// context. Will NOT be called if execution encounters a null Context exception.
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