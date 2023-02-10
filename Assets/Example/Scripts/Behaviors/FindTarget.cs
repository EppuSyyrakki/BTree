using BTree;
using UnityEngine;

public class FindTarget : Leaf<ITreeContext>
{
    private Player player;

    protected override void OnSetup()
    {
        player = Agent as Player;
    }

    protected override void OnEnter() { }

    protected override void OnExecute()
    {
        var dist = Vector3.Distance(player.Ball.Position, player.Position);

        if (dist > 2f)
        {
            Response.Result = Result.Failure;
            return;
        }

        if (!IsBlocked(player.OpponentGoal.Position))
        {
            Context = player.OpponentGoal;
            Draw(player.Position, Context.Position);
        }
        else if (FindTeamMate(out var mate))
        {
            Context = mate;
            Draw(player.Position, Context.Position);
        }
        else
        {
            Response.Result = Result.Failure;
        }

        Response.Result = Result.Success;
    }

    protected override void OnExit() { }

    protected override void OnReset() { }

    protected override void OnFail() { }

    private bool IsBlocked(Vector3 target)
    {
        var dir = target - player.Ball.Position;
        var hits = Physics.RaycastAll(player.Ball.Position, dir, dir.magnitude);

        foreach (var hit in hits)
        {
            if (hit.transform.TryGetComponent<Player>(out var found) && found.Side != player.Side)
            {
                return true;
            }
        }

        return false;
    }

    private bool FindTeamMate(out Player teamMate)
    {
        foreach (var player in player.TeamMates)
        {
            if (IsBlocked(player.Position)) { continue; }

            teamMate = player;
            return true;
        }

        teamMate = null;
        return false;
    }

    private void Draw(Vector3 a, Vector3 b)
    {
        var color = player.Side == Player.Team.Blue ? Color.blue : Color.red;
        Debug.DrawLine(a, b, color, 2f);
    }
}
