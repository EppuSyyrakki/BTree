using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace BTree
{
    public abstract class Branch : TreeNode
    {
        [SerializeField, Input(dynamicPortList: true, connectionType: ConnectionType.Override)]
        private TreeResponse conditions;

        protected List<Condition> conditionNodes = null;
        protected TreeResponse storedResponse = null;

        private bool CheckOwnConditions()
        {
            foreach (var condition in conditionNodes)
            {
                if (!condition.Check())
                {
                    return false;
                }
            }

            return true;
        }

        protected TreeResponse ResolveConditions(TreeResponse response)
        {
            if (response.Result != Result.Failure)
            {
                // Not a failure, so check conditions
                if (!CheckOwnConditions())
                {
                    // Conditions failed, so pass failure
                    response.Result = Result.Failure;
                    response.Conditions.Clear();
                    storedResponse = response;
                    return storedResponse;
                }

                // conditions passed
                response.Conditions.AddRange(conditionNodes);
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
    }
}
