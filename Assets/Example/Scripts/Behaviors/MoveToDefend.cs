using BTree;
using UnityEngine;

/// <summary>
/// Example of using the Out Context field on the node.
/// </summary>
public class MoveToDefend : Leaf<NoContext>
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
        base.Execute();

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
    }
    protected override void OnExit()
    {
        player.MovingToBall = false;
    }

    protected override void ResetLeaf()
    {
        player = null;
        target = default;
    }

    private Vector3 GetLocation()
    {
        Vector3 between = (player.Ball.transform.position - player.OwnGoal.transform.position) * 0.5f;
        return player.OwnGoal.transform.position + between;
    }
}
