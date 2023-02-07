using System.Collections.Generic;
using XNode;
using UnityEngine;

namespace BTree
{
    /// <summary>
    /// 
    /// </summary>    
	public class While : TreeNode
	{
        [SerializeField, Input(dynamicPortList: true, connectionType: ConnectionType.Override)]
        private TreeResponse conditions;

        [SerializeField, Input(dynamicPortList: false, connectionType: ConnectionType.Override)]
        private TreeResponse input;

        private List<Condition> conditionNodes = null;
        private TreeResponse storedResponse = null;

        public override object GetValue(NodePort port)
		{
            if (!initialized) { return null; }
            
            if (storedResponse != null) { return storedResponse; }

            var response = GetChildResponse();

            if (response.Result != Result.Failure)
            {
                // Not a failure, so check conditions
                response.Conditions.AddRange(conditionNodes);

                if (!response.CheckConditions())
                {
                    // Conditions failed, so pass failure
                    response.Result = Result.Failure;
                    response.Conditions.Clear();
                    storedResponse = response;
                    return storedResponse;
                }
            }
            else
            {
                // Failure, cache response and send. No need to check conditions.
                storedResponse = response;
                return response;
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

                if (port.Connection.node is TreeNode node)
                {
                    if (node is Condition condition)    // add conditions to a separate list.
                    {
                        condition.Host = this;
                        conditionNodes.Add(condition);                        
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