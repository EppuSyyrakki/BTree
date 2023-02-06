using System.Collections.Generic;
using XNode;
using UnityEngine;

namespace BTree
{
    /// <summary>
    /// 
    /// </summary>    
	public class Parallel : TreeNode
	{
        [SerializeField, Input(dynamicPortList: true, connectionType: ConnectionType.Override)]
        private TreeResponse conditions;

        [SerializeField, Input(dynamicPortList: false, connectionType: ConnectionType.Override)]
        private TreeResponse input;

        private List<Condition> conditionNodes;
        private TreeResponse storedResponse = null;

        public override object GetValue(NodePort port)
		{
            if (!initialized) { return null; }
            
            if (storedResponse != null) { return storedResponse; }

            var response = GetChildResponse();

            if (response.Result != Result.Failure)
            {
                response.Conditions.AddRange(conditionNodes);
            }            

            if (!response.CheckConditions())
            {
                response.Result = Result.Failure;
                response.Conditions.Clear();
                storedResponse = response;
                return storedResponse;
            }

            return response;
		}

        protected override TreeNode[] GetChildNodes()    // Find all nodes connected to childPort ports.
        {
            List<TreeNode> connectedChildren = new List<TreeNode>();
            conditionNodes = new List<Condition>();

            foreach (var port in Inputs)
            {
                if (port.Connection == null) { continue; }

                var node = port.Connection.node as TreeNode;

                if (node != null)
                {
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

        internal override void ResetNode()
        {
            storedResponse = null;
        }
    }
}