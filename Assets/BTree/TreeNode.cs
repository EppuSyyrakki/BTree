using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

namespace BTree
{
	[NodeWidth(250)]
	public abstract class TreeNode : Node
	{
		[SerializeField, Tooltip("A visual representation of the value received from children")]
		protected Result graphResult;

		[Output(connectionType = ConnectionType.Override)]
		public NodeResult parentPort;
		
		protected SceneTree Tree;

		[HideInInspector]
		public TreeNode parent;
		[HideInInspector]
		public TreeNode[] children;

		/// <summary>
		/// Use this instead of Node.Init() to reset all variables and get child node(s).
		/// <param name="t">The tree that has this node in it.</param>
		/// </summary>
		public virtual void Setup(SceneTree t)
		{
			children = GetChildNodes();

			if (Tree == null || Tree != t) { Tree = t; }
		}

		private TreeNode[] GetChildNodes()    // Find all nodes connected to childPort ports.
		{
			var ports = Inputs.ToArray();
			List<TreeNode> connectedChildren = new List<TreeNode>();

			foreach (var port in ports)
			{
				if (port.Connection == null) { continue; }

				var node = port.Connection.node as TreeNode;

				if (node != null)
				{
					node.parent = this;
					connectedChildren.Add(node);
				}
			}

			return connectedChildren.ToArray();
		}

		/// <summary>
		/// Convenience method to get index 0 child.
		/// </summary>
		/// <returns></returns>
		public NodeResult GetFirstChildResult()
		{
			return GetChildResultAtIndex(0);
		}

		/// <summary>
		/// Get value (result) from first connected child.
		/// </summary>
		/// <returns>The result of the first child node. Null if no children found.</returns>
		protected NodeResult GetChildResultAtIndex(int i)
		{
			if (children == null || children.Length == 0) { return null; }  // Check to prevent editor errors

			// Fetch the port on the child that leads to this node and return the value it gets.
			var child = children[i];
			var childOutput = child.GetOutputPort(nameof(child.parentPort));
			var result = child.GetValue(childOutput) as NodeResult;
			return result;
		}

		public void RecursiveResetChildren()
		{
			if (children == null) { return; }

			foreach (var treeNode in children)
			{
				treeNode.ResetNode();
				treeNode.RecursiveResetChildren();
			}
		}

		/// <summary>
		/// Use this to reset any member variables in inheriting classes.
		/// </summary>
		public virtual void ResetNode()
		{
			graphResult = Result.Running;
		}
	}
}