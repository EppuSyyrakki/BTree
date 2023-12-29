using BTree;
using UnityEngine;

namespace Conditions
{
    /// <summary>
    /// Example of using Condition nodes.
    /// </summary>
    public class IsClosestToBall : Condition
    {
        protected override bool OnCheck()
        {
            if (Agent == null) { return false; }

            var player = Agent as Player;

            if (IsClosest(player))
            {
                return true;
            }

            return false;
        }

        private static bool IsClosest(Player player)
        {
            if (player.Ball == null) { return false; }

            Vector3 ball = player.Ball.Position;
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
