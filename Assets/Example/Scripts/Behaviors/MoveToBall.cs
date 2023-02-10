﻿using BTree;
using UnityEngine;

/// <summary>
/// Example of using the Out Context field on the node.
/// </summary>
public class MoveToBall : Leaf<ITreeContext>
{
    private Player player;
    private Vector3 target;

    protected override void OnSetup()
    {
        player = Agent as Player;
    }

    protected override void OnEnter()
    {
        var pos = GetLocation();
        target = player.MoveTo(pos);
        player.MovingToBall = true;
    }

    protected override void OnExecute()
    {
        if (player.Ball == null)
        {
            Response.Result = Result.Failure;
            return;
        }

        if ((target - player.Ball.Position).sqrMagnitude > 2f)
        {
            var pos = GetLocation();
            target = player.MoveTo(pos);
        }

        if ((player.transform.position - player.Ball.Position).sqrMagnitude < 3f)
        {
            Response.Result = Result.Success;
        }
    }

    protected override void OnExit()
    {
        player.MovingToBall = false;
    }

    private Vector3 GetLocation()
    {
        var goalToBall = player.Ball.Position - player.OpponentGoal.Position;
        var behindBall = player.Ball.Position + (goalToBall.normalized * 0.75f);

        return behindBall;
    }

    protected override void OnReset()
    {
        target = default;
    }

    protected override void OnFail() { }
}
