using System.Collections.Generic;
using UnityEngine;

namespace BTree
{
	/// <summary>
	/// An instantiated version of the TreeAsset graph that can be used and referenced in scenes at runtime.
	/// </summary>
	public class TreeAgent : MonoBehaviour, ITreeContext
	{
        [SerializeField, Tooltip("Reference to a TreeAsset ScriptableObject")]
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

        protected virtual void Update()
        {
			if (current == null)
            {
                Evaluate(out current);
                current.Origin.Enter();
            }

            if (current.CheckConditions())
            {
                current.Origin.Execute();
            }

            if (current.Result == Result.Running) { return; }

			current.Origin.Exit();

            if (Evaluate(out TreeResponse next))
            {
                next.Origin.Enter();
                current = next;
            }
			else
			{
                Restart();
                current = null;
            }
        }

		/// <summary>
		/// Resets all nodes and clears the context dictionary. Used when the agent can't find any Running results.
		/// </summary>
        protected void Restart()
        {
			if (debugTree) { Debug.Log(gameObject.name + " is resetting its behavior tree."); }

			context.Clear();
			Tree.ResetNodes();
        }        

		/// <summary>
		/// Recursively travel the tree to find Result.Running.
		/// </summary>
		/// <param name="response">The response from the tree. Null if nothing found.</param>
		/// <returns>True if runnable response found, false if not.</returns>
		internal bool Evaluate(out TreeResponse response)
        {
			if (debugTree) { Debug.Log(gameObject.name + " evaluating tree..."); }
		
            // Recursively travel the tree toward first waiting result.
            response = Tree.Root.Response;

            if (response.Result != Result.Running || response.Origin == null) 
			{
				if (debugTree) { Debug.Log($"{gameObject.name} received {response.Result} from {response.Origin}"); }
                return false;
			}

            if (debugTree) { DebugResult(response); }

			return true;
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

        private void DebugResult(TreeResponse result)
        {
            if (result?.Origin != null)
            {
                Debug.Log($"{gameObject.name} SceneTree result: {result.Result} from {result.Origin}");
            }
        }
    }
}