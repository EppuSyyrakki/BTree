using System.Collections.Generic;
using XNode;
using UnityEngine;
using System.Linq;

namespace BTree
{
    /// <summary>
    /// 
    /// </summary>    
	public class Parallel : TreeNode
	{
        [SerializeField, Input(dynamicPortList: true, connectionType: ConnectionType.Override)]
        private Result conditions;

        [SerializeField, Input(dynamicPortList: false, connectionType: ConnectionType.Override)]
        private TreeResult input;

        private List<Condition> conditionNodes;

        internal override void Setup(Tree t)
        {
            tree = t;
            children = GetChildNodes();
        }

        public override object GetValue(NodePort port)
		{
            var result = GetResult();
            result.Conditions.AddRange(conditionNodes);

            if (!result.CheckConditions())
            {
                result.Value = Result.Failure;
            }

            return result;
		}

        private TreeNode[] GetChildNodes()    // Find all nodes connected to childPort ports.
        {
            List<TreeNode> connectedChildren = new List<TreeNode>();
            conditionNodes = new List<Condition>();

            foreach (var port in Inputs)
            {
                if (port.Connection == null) { continue; }

                var node = port.Connection.node as TreeNode;

                if (node != null)
                {
                    node.parent = this;

                    if (node is Condition c)    // add conditions to a separate list.
                    {
                        c.Host = this;
                        conditionNodes.Add(c);                        
                    }
                    else
                    {
                        connectedChildren.Add(node);
                    }                   
                }
            }

            return connectedChildren.ToArray();
        }
    }
}