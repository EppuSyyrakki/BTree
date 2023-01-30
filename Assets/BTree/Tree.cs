using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BTree
{
	/// <summary>
	/// An instantiated version of the TreeAsset graph that can be used and referenced in scenes at runtime.
	/// </summary>
	public class Tree : SceneGraph
	{
        [SerializeField, Tooltip("Reference to the Behaviour Tree ScriptableObject")]
        private TreeAsset treeAsset;

        private RootNode root;
		private Dictionary<string, ITreeContext> context;

		internal TreeAgent agent;

        public bool debugTree = false;

		private void Awake()
		{
			if (treeAsset == null)
			{
				Debug.LogError($"SceneTree on GameObject {name} has no TreeAsset assigned!");
				return;
			}
			else
			{
                graph = treeAsset.Copy();
				context = new Dictionary<string, ITreeContext>();
				InitGraph();
            }

			agent = GetComponent<TreeAgent>();
		}

		private void DebugResult(TreeResult result)
		{
			if (result?.Origin != null)
			{			
				Debug.Log($"{gameObject.name} SceneTree result: {result.Value} from {result.Origin}");
			}
		}

        private void InitGraph()
        {
            foreach (var n in graph.nodes)
            {
                var tn = n as TreeNode;

                if (tn == null)
                {
                    Debug.LogError(gameObject.name + " has non-TreeNode in their tree.");
                    return;
                }

                if (tn is RootNode rootNode)
                {
                    root = rootNode;
                }
				else if (tn is Restart reset)
				{

				}

                tn.Setup(this);
            }
        }

		/// <summary>
		/// Resets all nodes and clears the context dictionary. Used when the agent can't find any Running results.
		/// </summary>
        internal void Restart()
        {
			if (debugTree) { Debug.Log(gameObject.name + " is resetting its behavior tree."); }

			context.Clear();

            foreach (var n in graph.nodes)
            {
                var tn = n as TreeNode;
                tn.ResetNode();
            }
        }        

		/// <summary>
		/// Recursively travel the tree to find Result.Running.
		/// </summary>
		/// <param name="leaf">The new leaf found from the tree. Null if nothing found.</param>
		/// <returns>True if runnable leaf found, false if not.</returns>
		internal bool Evaluate(out TreeResult result)
        {
			if (debugTree) { Debug.Log(gameObject.name + " evaluating tree..."); }
		
            // Recursively travel the tree toward first running result.
            result = root.Result;

            if (result.Value != Result.Running || result.Origin == null) 
			{
				if (debugTree) { Debug.Log($"{gameObject.name} received {result.Value} from {result.Origin}"); }
                return false;
			}

            if (debugTree) { DebugResult(result); }

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
	}
}