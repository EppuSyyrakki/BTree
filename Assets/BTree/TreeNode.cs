using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

namespace BTree
{
	[NodeWidth(275)]
	public abstract class TreeNode : Node
	{
        [SerializeField, Output(connectionType = ConnectionType.Override)]
        protected TreeResponse output;

        protected Tree tree;		
		protected TreeNode[] children;

        internal TreeNode parent;

        /// <summary>
        /// Use this instead of Node.Init() to reset all variables and get child node(s).
        /// <param name="t">The tree that has this node in it.</param>
        /// </summary>
        internal virtual void Setup(Tree t)
		{
            tree = t;
            children = GetChildNodes();			
		}

		private TreeNode[] GetChildNodes()    // Find all nodes connected to childPort ports.
		{
            List<TreeNode> connectedChildren = new List<TreeNode>();           
			var inputs = Inputs.ToArray();

			foreach (var input in inputs)
			{
				if (input.Connection == null) { continue; }

				var node = input.Connection.node as TreeNode;

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
		protected TreeResponse GetChildResponse()
		{
			return GetChildResponseAtIndex(0);
		}

		/// <summary>
		/// Get value (result) from connected child at index i.
		/// </summary>
		/// <returns>The result of the child node at index i. Null if no children found.</returns>
		protected TreeResponse GetChildResponseAtIndex(int i)
		{
			if (children == null || children.Length == 0) { return null; }  // Check to prevent editor errors

			// Fetch the port on the child that leads to this node and return the value it gets.
			var child = children[i];
			var childOutput = child.GetOutputPort(nameof(child.output));
			var result = child.GetValue(childOutput) as TreeResponse;
			return result;
		}

		internal void RecursiveResetChildren()
		{
			if (children == null) { return; }

			foreach (var treeNode in children)
			{
				treeNode.ResetNode();
				treeNode.RecursiveResetChildren();
			}
		}

		internal void RecursiveFail()
		{
			if (this is ILeaf leaf)
			{
				leaf.Fail();
			}
			else
			{
				foreach(var child in children)
				{
					child.RecursiveFail();
				}
			}
		}

		/// <summary>
		/// Use this to reset any member variables in inheriting classes.
		/// </summary>
		internal virtual void ResetNode() { }
    }
}