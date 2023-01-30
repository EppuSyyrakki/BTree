using BTree;
using UnityEngine;
using XNode;

namespace Conditions
{
    /// <summary>
    /// Example of using Condition nodes.
    /// </summary>
    public class IsClosestToBall : Condition
    {
        public override object GetValue(NodePort port)
        {
            if (tree == null) { return null; }

            var player = tree.agent as Player;

            if (IsClosest(player))
            {
                return new ConditionResult(Result.Success);
            }

            if (tree.debugTree) { Debug.Log($"{tree.agent} condition {this} failed!"); }

            return new ConditionResult(Result.Failure);
        }

        private static bool IsClosest(Player player)
        {
            if (player.Ball == null) { return false; }

            Vector3 ball = player.Ball.transform.position;
            float self = (player.transform.position - ball).sqrMagnitude;
            float closest = float.MaxValue;

            foreach (var p in player.TeamMates)
            {
                float friend = (p.transform.position - ball).sqrMagnitude;

                if (friend < closest)
                {
                    closest = friend;
                }
            }
           
            return self < closest;
        }
    }
}
