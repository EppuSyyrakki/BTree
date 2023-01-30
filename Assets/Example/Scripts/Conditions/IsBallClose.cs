using BTree;
using UnityEngine;
using XNode;

namespace Conditions
{
    /// <summary>
    /// Example of using Condition nodes.
    /// </summary>
    public class IsBallClose : Condition
    {
        [SerializeField]
        private float distance = 3f;

        public override object GetValue(NodePort port)
        {
            if (tree == null) { return null; }

            var player = tree.agent as Player;

            if (Vector3.Distance(player.Ball.transform.position, player.transform.position) < distance)
            {
                return new ConditionResult(Result.Success);
            }

            if (tree.debugTree) { Debug.Log($"{tree.agent} condition {this} failed!"); }

            return new ConditionResult(Result.Failure);
        }
    }
}
