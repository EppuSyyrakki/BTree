using BTree;
using UnityEngine;
using XNode;

namespace Conditions
{
    /// <summary>
    /// Example of using a Condition in a tree.
    /// </summary>
    public class IsFriendMovingToBall : Condition
    {
        public override object GetValue(NodePort port)
        {
            if (tree == null) { return null; }

            var player = tree.agent as Player;

            foreach (var friend in player.TeamMates)
            {
                if (friend.MovingToBall)
                {
                    return new ConditionResult(Result.Success);
                }
            }

            if (tree.debugTree) { Debug.Log($"{tree.agent} condition {this} failed!"); }

            return new ConditionResult(Result.Failure);
        }
    }
}
