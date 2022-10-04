using System;
using System.Collections.Generic;
using System.Linq;
using BTree.Agent;
using BTree.Nodes;
using UnityEngine;
using XNode;
using Random = UnityEngine.Random;

namespace BTree
{
	public class SceneTree : SceneGraph
	{
        [SerializeField]
        private BehaviourTreeAsset treeAsset;

        [SerializeField]
        private float evaluationInterval = 0.25f;

        [SerializeField]
        private bool debugTree = false;

        private RootNode root;
		private NodeResult currentResult;
		private bool pathChanged;
		private Timer evaluateTimer;

		public BTreeAgent Agent { get; private set; }

		public event Action PathChanged;

		private void Awake()
		{
			Agent = GetComponent<BTreeAgent>();
			graph = treeAsset.Copy();
			evaluateTimer = new Timer(time: evaluationInterval, autoReset: true, 
				initialTime: Random.Range(0, evaluationInterval));
		}

		private void OnEnable()
		{
			evaluateTimer.Alarm += Evaluate;
		}

		private void OnDisable()
		{
			evaluateTimer.Alarm -= Evaluate;
		}

		private void Start()
		{
			if (graph != null) { InitGraph(); }
			else { Debug.LogWarning(gameObject.name + " has a null tree graph"); }

			EvaluateFirst();
		}

		private void Update()
		{
			if (pathChanged)
			{
				PathChanged?.Invoke();
				pathChanged = false;
			}

			evaluateTimer.Update();
		}

		private void EvaluateFirst()
		{
			var result = root.GetFirstChildResult(); // Recursively travel the tree toward first running result.

			if (result.Value != Result.Running) { return; } // No runnable results found, ignore.
			
			currentResult = result; // Copy result path to current, invoke event on next Update.

			if (debugTree) { DebugResult(result); }
		}

		private void Evaluate()
		{
			var result = root.GetFirstChildResult(); // Recursively travel the tree toward first running result.

			if (result.Value != Result.Running) { return; } // No runnable results found, ignore.

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

		private void DebugResult(NodeResult result)
		{
			string n = gameObject.name;

			if (result == null)
			{
				Debug.LogWarning(n + " AgentTree result: null");
				return;
			}

			if (result.Origin != null)
			{
				string path = "";

				foreach (var node in result.Path)
				{
					path += " <- " + node.name;
				}
				
				Debug.Log(n + " AgentTree result: " + result.Value + path);
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

		public Leaf GetCurrentLeaf()
		{
			var leaf = currentResult.Origin as Leaf;

			if (leaf != null) return leaf;
			
			Debug.LogWarning(gameObject.name + " has a non-Leaf node as the current origin.");
			return null;
		}

		public void SetCurrentLeafResult(Result r)
		{
			if (currentResult.Origin is Leaf l) { l.SetCurrentResult(r); }
		}
	}
}