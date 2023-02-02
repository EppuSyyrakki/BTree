using BTree;
using UnityEngine;

namespace Conditions
{
    /// <summary>
    /// Example of using a Condition in a tree.
    /// </summary>
    public class IsFriendDefending : Condition
    {
        protected override bool OnCheck()
        {
            if (tree == null) { return false; }

            var player = tree.agent as Player;

            foreach (var friend in player.TeamMates)
            {
                if (friend.IsDefending)
                {
                    return true;
                }
            }

            if (tree.debugTree) { Debug.Log($"{tree.agent} condition {this} failed!"); }

            return false;
        }
    }
}
