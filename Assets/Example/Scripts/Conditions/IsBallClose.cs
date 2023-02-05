﻿using BTree;
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
            if (Agent == null) { return false; }

            var player = Agent as Player;

            if (Vector3.Distance(player.Ball.transform.position, player.transform.position) < distance)
            {
                return true;
            }

            if (Agent.debugTree) { Debug.Log($"{Agent} condition {this} failed!"); }

            return false;
        }
    }
}
