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
            Debug.DrawLine(player.Position, Context.Position, Color.black, 2f);
        }
        else if (player.TeamMates.Count > 0)
        {
            Context = FindTeamMate();
            var color = player.Side == Player.Team.Blue ? Color.blue : Color.red;
            Debug.DrawLine(player.Position, Context.Position, color, 2f);
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

    private Player FindTeamMate()
    {
        foreach (var player in player.TeamMates)
        {
            if (IsBlocked(player.Position)) { continue; }

            return player;
        }

        return null;
    }
}
