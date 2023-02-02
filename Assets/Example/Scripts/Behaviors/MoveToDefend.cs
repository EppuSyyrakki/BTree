using BTree;
using UnityEngine;

/// <summary>
/// Example of using the Out Context field on the node.
/// </summary>
public class MoveToDefend : Leaf<NoContext>
{
    private Player player;
    private Vector3 target;
    private float timer;

    protected override void OnEnter()
    {
        player = Agent as Player;
        player.IsDefending = true;
        var pos = GetLocation();
        target = player.MoveTo(pos);
        timer = 0;
    }

    public override void Execute()
    {     
        if (player.Ball == null)
        {
            Result = Result.Failure;
            return;
        }

        if ((target - player.Ball.transform.position).sqrMagnitude > 3f)
        {
            var pos = GetLocation();
            target = player.MoveTo(pos);
            return;
        }

        if ((player.transform.position - target).sqrMagnitude < 1.5f)
        {
            player.IsDefending = true;
            timer += Time.deltaTime;

            if (timer > maxDuration)
            {
                Result = Result.Success;
                player.IsDefending = false;
            }
        }
        else
        {
            player.IsDefending = false;
        }
    }
    protected override void OnExit() { }

    internal override void ResetNode()
    {
        base.ResetNode();
        player = null;
        target = default;
    }

    private Vector3 GetLocation()
    {
        Vector3 between = (player.Ball.transform.position - player.OwnGoal.transform.position) * 0.5f;
        return player.OwnGoal.transform.position + between;
    }
}
