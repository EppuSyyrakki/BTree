﻿using System.Collections.Generic;
using UnityEngine;

namespace BTree
{
	/// <summary>
	/// An instantiated version of the TreeAsset graph that can be used and referenced in scenes at runtime.
	/// </summary>
	public class TreeAgent : MonoBehaviour, ITreeContext
	{
        [SerializeField, Tooltip("Reference to a TreeAsset ScriptableObject. Will get copied to a property on Awake.")]
        private TreeAsset treeAsset;

		protected Dictionary<string, ITreeContext> context;
        protected TreeResponse current;
		
		public TreeAsset Tree { get; protected set; }
        public bool debugTree = false;

		protected virtual void Awake()
		{
            if (treeAsset == null)
			{
				Debug.LogError($"TreeAgent on GameObject {name} has no TreeAsset assigned!");
				return;
			}
			else
			{
				context = new Dictionary<string, ITreeContext>();
				Tree = (TreeAsset)treeAsset.Copy();
				Tree.Initialize(this);
            }
		}

		protected virtual void Start()
		{
            Tree.Evaluate(out current);
            current.Origin.Enter();
        }

        protected virtual void Update()
        {
            if (current.CheckConditions())
            {
                current.Origin.Execute();
            }

            if (current.Result == Result.Running) { return; }

			current.Origin.Exit();

            if (Tree.Evaluate(out TreeResponse next))
            {
                next.Origin.Enter();
                current = next;
            }
			else
			{
                Restart();
				Tree.Evaluate(out current);
				current.Origin.Enter();
            }
        }

		/// <summary>
		/// Resets all nodes and clears the context dictionary. Used when the agent can't find any Running results.
		/// </summary>
        protected void Restart()
        {
			context.Clear();
			Tree.ResetNodes();
        }        
		
		public void TriggerInterrupt(string interruptId, ITreeContext context)
		{
			if (!Tree.TryInterrupt(interruptId, context, out Interrupt interruption)) { return; }
			
			if (debugTree) { Debug.Log($"{name} was interrupted with Id {interruptId}"); }

            if (!string.IsNullOrEmpty(interruption.OutContext))
            {
                if (context == null)
                {
                    if (debugTree)
                    {
                        Debug.LogWarning($"{name}.{interruption} outContext {interruption.OutContext} not set.");
                    }
                }
                else if (TryAddContext(interruption.OutContext, context, interruption.OverwriteOut))
                {
                    if (debugTree)
                    {
                        Debug.LogWarning($"{name}.{interruption} outContext {interruption.OutContext} already exists.");
                    }
                }
            }

            if (current.Origin != null)
			{
				current.Origin.Fail();
				Tree.Evaluate(out current);
				current.Origin.Enter();
            }
			else
			{
				// TODO: See if this is even necessary.
				current.Result = Result.Failure;
			}			
		}

		/// <summary>
		/// Tries to add a new context key/value pair.
		/// </summary>
		/// <param name="key">The dictionary key to add.</param>
		/// <param name="context">The context to add under the key.</param>
		/// <param name="overwrite">If true and key already exists, it will be overridden by the context.</param>
		/// <returns>True if the key/value was added.</returns>
		public bool TryAddContext(string key, ITreeContext context, bool overwrite)
		{
            if (!overwrite && this.context.ContainsKey(key)) { return false; }

			this.context[key] = context;
			return true;
        }

		/// <summary>
		/// Tries to fetch a context from the dictionary.
		/// </summary>
		/// <param name="key">The key to fetch with.</param>
		/// <param name="context">The found context. Null if key not found.</param>
		/// <returns>True if the key was found.</returns>
		public bool TryGetContext(string key, out ITreeContext context)
		{
			if (this.context.TryGetValue(key, out context)) { return true; }

			return false;
		}

		/// <summary>
		/// Tries to remove all keys that have a certain context as their value.
		/// </summary>
		/// <param name="context">The context to remove.</param>
		/// <returns>True if the context was removed.</returns>
		public bool RemoveContext(ITreeContext context)
		{
			List<string> keysToRemove = new List<string>();
			bool removed = false;

			foreach (var pair in this.context)
			{
				if (pair.Value == context)
				{					
					keysToRemove.Add(pair.Key);
					removed = true;
				}
			}

			foreach (var key in keysToRemove)
			{
                this.context.Remove(key);
			}

			return removed;
		}

		/// <summary>
		/// Tries to remove a context from the context dictionary.
		/// </summary>
		/// <param name="key">The key to remove.</param>
		/// <returns>True if the key was removed.</returns>
		public bool RemoveContext(string key)
		{
			return this.context.Remove(key);
		}
    }
}