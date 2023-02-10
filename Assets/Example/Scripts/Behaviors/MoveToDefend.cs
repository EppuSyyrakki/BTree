using BTree;
using UnityEngine;

public class MoveToDefend : Leaf<ITreeContext>
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

    protected override void OnExecute()
    {
        if (player.Ball == null)
        {
            Response.Result = Result.Failure;
            return;
        }

        Vector3 defensePos = GetLocation();

        if ((target - defensePos).magnitude > 1f)
        {
            player.IsDefending = false;
            target = player.MoveTo(defensePos);
            return;
        }

        player.IsDefending = true;
        timer += Time.deltaTime;

        if (timer > maxDuration)
        {
            Response.Result = Result.Success;
            player.IsDefending = false;
        }
    }

    protected override void OnExit()
    {
        player.IsDefending = false;
    }

    private Vector3 GetLocation()
    {
        Vector3 goalToBall = player.Ball.Position - player.OwnGoal.Position;
        Vector3 offset;

        if (goalToBall.magnitude > player.DefensiveRange)
        {
            offset = goalToBall.normalized * player.DefensiveRange;
        }
        else
        {
            offset = goalToBall * player.DefensiveRange * 0.1f;
        }
        return player.OwnGoal.Position + offset;
    }

    protected override void OnReset()
    {
        target = default;
    }

    protected override void OnFail() { }
}
