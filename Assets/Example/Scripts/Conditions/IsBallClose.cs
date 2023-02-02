using BTree;
using UnityEngine;

namespace Conditions
{
    /// <summary>
    /// Example of using Condition nodes.
    /// </summary>
    public class IsBallClose : Condition
    {
        [SerializeField]
        private float distance = 3f;

        protected override bool OnCheck()
        {
            if (tree == null) { return false; }

            var player = tree.agent as Player;

            if (Vector3.Distance(player.Ball.transform.position, player.transform.position) < distance)
            {
                return true;
            }

            if (tree.debugTree) { Debug.Log($"{tree.agent} condition {this} failed!"); }

            return false;
        }
    }
}
