using BTree;
using UnityEngine;

/// <summary>
/// Example of using the Out Context field on the node.
/// </summary>
public class MoveToBall : Leaf<NoContext>
{
    private Player player;
    private Vector3 target;

    protected override void OnEnter()
    {
        player = Agent as Player;
        var pos = GetLocation();
        target = player.MoveTo(pos);
        player.MovingToBall = true;
    }

    public override void Execute()
    {
        if (player.Ball == null)
        {
            Result = Result.Failure;
            return;
        }

        if ((target - player.Ball.transform.position).sqrMagnitude > 2.5f)
        {
            var pos = GetLocation();
            target = player.MoveTo(pos);
            return;
        }

        if ((player.transform.position - target).sqrMagnitude < 1.5f)
        {           
            Result = Result.Success;
        }

        base.Execute();
    }
    protected override void OnExit()
    {
        player.MovingToBall = false;
        player.MoveTo(player.transform.position);
    }

    protected override void ResetLeaf()
    {
        player = null;
        target = default;
    }

    private Vector3 GetLocation()
    {
        var goalToBall = player.Ball.transform.position - player.OpponentGoal.transform.position;
        var behindBall = player.Ball.transform.position + (goalToBall.normalized * 0.75f);

        return behindBall;
    }
}
