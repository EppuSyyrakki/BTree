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

        internal void InitGraph()
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

				tn.Setup(this);
			}
		}

		/// <summary>
		/// Recursively travel the tree to find Result.Running.
		/// </summary>
		/// <param name="leaf">The new leaf found from the tree. Null if nothing found.</param>
		/// <returns>True if leaf found, false if not.</returns>
		internal bool Evaluate(out ILeaf leaf)
        {
			if (debugTree) { Debug.Log(gameObject.name + " evaluating tree..."); }

			leaf = null;
            var result = root.GetResult(); // Recursively travel the tree toward first running result.

            if (result.Value == Result.Failure) 
			{
                if (debugTree) { Debug.Log(gameObject.name + " found only a Failure result."); }
                return false;
			}

            if (debugTree) { DebugResult(result); }

			leaf = result.Origin;
			return true;
        }

		public bool TryAddContext(string key, ITreeContext context, bool overwrite)
		{
            if (!overwrite && this.context.ContainsKey(key)) { return false; }

			this.context[key] = context;
			return true;
        }

		public bool TryGetContext(string key, out ITreeContext context)
		{
			if (this.context.TryGetValue(key, out context)) { return true; }

			return false;
		}

		public void RemoveContext(ITreeContext context)
		{
			List<string> keysToRemove = new List<string>();

			foreach (var pair in this.context)
			{
				if (pair.Value == context)
				{
					keysToRemove.Add(pair.Key);
				}
			}

			foreach (var key in keysToRemove)
			{
				this.context.Remove(key);
			}
		}
	}
}