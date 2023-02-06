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

		[SerializeField, Tooltip("")]
		private bool overwriteOut = true;

        private float elapsed = 0;		
		private T context;

        public TreeResponse Response { get; set; }
        protected T Context
        {
            get
            {
                if (context is NoContext)
                {
                    Debug.LogError($"{Agent}.{this} trying to use context while having NoContext type");
                    return null;
                }

                return context;
            }
            set => context = value;
        }

        internal sealed override void Setup(TreeAgent agent)
        {
            Response = new TreeResponse(this);
            base.Setup(agent);
            OnSetup();
        }

        protected abstract void OnSetup();

		public void Enter()
		{
            elapsed = 0;
            Response.Result = Result.Running;

            if (!string.IsNullOrEmpty(inContext))
            {
                if (Agent.TryGetContext(inContext, out ITreeContext context))
                {
                    this.context = context as T;
                }
                else
                {
                    if (Agent.debugTree)
                    {
                        Debug.LogWarning($"{Agent}.{this} could not find InputVariable {inContext}.");
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
        /// Override this to create actionable nodes that get sent to the agent after the tree is evaluated. 
        /// The base implementation only advances the timer.
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
                if (context == null)
                {
                    if (Agent.debugTree)
                    {
                        Debug.LogWarning($"{Agent} TreeAgent: context {outContext} not set.");
                    }
                    
                    return;
                }

                if (!Agent.TryAddContext(outContext, context, overwriteOut))
                {
                    if (Agent.debugTree)
                    {
                        Debug.LogWarning($"{Agent} TreeAgent: context {outContext} already exists.");
                    }                   
                }                
            }
        }

        /// <summary>
        /// Called by the base class before setting the context to the tree with the Out Context key, so any
        /// required context can be set inside this method.
        /// </summary>
        protected abstract void OnExit();

        internal sealed override void ResetNode()
		{
            Response.Result = Result.Running;
            context = null;
            elapsed = 0;
            OnReset();
		}

        protected abstract void OnReset();

        public void Fail()
        {
            Response.Result = Result.Failure;
            context = null;
        }

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