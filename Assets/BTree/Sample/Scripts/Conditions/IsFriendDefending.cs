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
            if (!initialized) { return false; }

            var player = Agent as Player;

            foreach (var friend in player.TeamMates)
            {
                if (friend.IsDefending)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
