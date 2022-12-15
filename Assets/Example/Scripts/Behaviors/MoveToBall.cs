using BTree;
using UnityEngine;

/// <summary>
/// Example of using the Out Context field on the node.
/// </summary>
public class MoveToBall : Leaf<Ball>
{
    private Player player;
    private Vector3 ballLocation;

    protected override void OnEnter()
    {
        player = agent as Player;
        ballLocation = context.transform.position;
        player.MoveTo = ballLocation;
    }

    public override void Execute()
    {
        base.Execute();

        if (player.Ball == null)
        {
            Result = Result.Failure;
            return;
        }

        if ((ballLocation - context.transform.position).sqrMagnitude > 0.9f)
        {
            ballLocation = player.Ball.transform.position;
            player.MoveTo = ballLocation;
            return;
        }

        if ((player.transform.position - player.Ball.transform.position).sqrMagnitude < 0.9f)
        {           
            Result = Result.Success;
        }
    }
    protected override void OnExit()
    {
        // The context can be added here because the base class calls this before setting the context
        // to the tree.
        context = player.Ball;
    }

    protected override void ResetLeaf()
    {
        player = null;
        ballLocation = default;
    }
}
