using System;
using System.Collections.Generic;
using System.Linq;
using BTree.Nodes;
using UnityEngine;
using XNode;
using Random = UnityEngine.Random;

namespace BTree
{
	/// <summary>
	/// An instantiated version of the TreeAsset graph that can be used and referenced in scenes at runtime.
	/// </summary>
	public class Tree : SceneGraph
	{
        [SerializeField, Tooltip("Reference to the Behaviour Tree ScriptableObject")]
        private TreeAsset treeAsset;

        [SerializeField, Tooltip("How often will the graph get evaluated")]
        private float timer = 0.1f;

        private RootNode root;
        private TreeResult currentResult;
        private bool pathChanged;
        private Timer evaluateTimer;

        public bool debugTree = false;
		public event Action<ActionNode> PathChanged;

		public Blackboard Blackboard { get; internal set; }

		private void Awake()
		{
			if (treeAsset == null)
			{
				Debug.LogError($"SceneTree on GameObject {name} has no TreeAsset assigned!");
			}
			else
			{
                graph = treeAsset.Copy();
            }
			
			evaluateTimer = new Timer(time: timer, autoReset: true, 
				initialTime: Random.Range(0, timer));
			evaluateTimer.Alarm += () => Evaluate(false);
		}

		private void Start()
		{
			InitGraph(); 
			Evaluate(firstPass: true);
		}

		private void Update()
		{
			if (pathChanged)
			{
				PathChanged?.Invoke(currentResult.OriginAsActionNode);
				pathChanged = false;
			}

			evaluateTimer.Update();
		}

		/// <summary>
		/// Recursively travel the tree to find Result.Running. If found, invoke PathChanged event on next frame.
		/// </summary>
		/// <param name="firstPass">If true, forces the result to currentResult field.</param>
		private void Evaluate(bool firstPass = false)
		{
			var result = root.GetResult(); // Recursively travel the tree toward first running result.

			if (result.Value != Result.Running) { return; } // No runnable results found, ignore.

			if (firstPass)
			{
                currentResult = result;
                
				if (debugTree) { DebugResult(result); }

				return;
            }

			if (result.Path.SequenceEqual(currentResult.Path)) { return; }
			
			pathChanged = true;
			ResetNodesExcept(result.Path);
			currentResult = result; // Copy result path to current, invoke event on next Update.

			if (debugTree) { DebugResult(result); }
		}

		private void ResetNodesExcept(List<TreeNode> except)
		{
			foreach (var node in graph.nodes)
			{
				var tn = node as TreeNode;

				if (tn is Selector s)
				{
					s.ResetNode(); 
					continue;
				}

				if (except.Contains(tn)) { continue; }
					
				tn.ResetNode();	// tn might be null but don't check - so we know where the problem is. 
			}
		}

		private void DebugResult(TreeResult result)
		{
			string n = gameObject.name;

			if (result == null)
			{
				Debug.LogWarning(n + " SceneTree result: null");
				return;
			}

			if (result.Origin != null)
			{
				string path = "";

				foreach (var node in result.Path)
				{
					path += " <- " + node.name;
				}
				
				Debug.Log(n + " SceneTree result: " + result.Value + path);
			}
		}

		private void InitGraph()
		{
			foreach (var n in graph.nodes)
			{
				var tn = n as TreeNode;

				if (tn == null) { Debug.LogWarning(gameObject.name + " has non-TreeNode in their tree."); return; }

				if (tn is RootNode root) { this.root = root; }

				tn.Setup(this);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		internal ActionNode GetActionNode()
		{
			var node = currentResult.OriginAsActionNode;

			if (node != null) return node;
			
			Debug.LogWarning(gameObject.name + " has a non-Leaf node as the current origin.");
			return null;
		}

		/// <summary>
		/// Called by the agent to match the current leaf result to the corresponding AgentAction result.
		/// </summary>
		/// <param name="r">The Result enum from AgentAction.Execute()</param>
		internal void SetActionNodeResult(Result r)
		{
			if (currentResult.Origin is ActionNode n) { n.SetCurrentResult(r); }
		}
	}
}