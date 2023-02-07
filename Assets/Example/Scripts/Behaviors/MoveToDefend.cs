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

    protected override void OnSetup()
    {
        player = Agent as Player;
    }

    protected override void OnEnter()
    {      
        var pos = GetLocation();
        target = player.MoveTo(pos);
        timer = 0;
    }

    public override void Execute()
    {     
        if (player.Ball == null)
        {
            Response.Result = Result.Failure;
            return;
        }

        Vector3 defensePos = GetLocation();

        if ((target - defensePos).magnitude > 1f)
        {
            target = player.MoveTo(defensePos);
            player.IsDefending = false;
            return;
        }

        if ((defensePos - player.transform.position).magnitude < 1f)
        {
            player.IsDefending = true;
            timer += Time.deltaTime;

            if (timer > maxDuration)
            {
                Response.Result = Result.Success;
                player.IsDefending = false;
            }
        }
    }
    protected override void OnExit() { }

    private Vector3 GetLocation()
    {
        Vector3 goalToBall = player.Ball.transform.position - player.OwnGoal.transform.position;
        Vector3 offset;

        if (goalToBall.magnitude > player.DefensiveRange)
        {
            offset = goalToBall.normalized * player.DefensiveRange;
        }
        else
        {
            offset = goalToBall * player.DefensiveRange * 0.1f;
        }
        return player.OwnGoal.transform.position + offset;
    }

    protected override void OnReset()
    {
        target = default;
    }

    protected override void OnFail() { }
}
