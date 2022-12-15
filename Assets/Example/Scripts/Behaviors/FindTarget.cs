using BTree;
using UnityEngine;

public class FindTarget : Leaf<ITreeContext>
{
    private Vector3 ball;
    private Player player;

    protected override void OnEnter()
    {        
        player = Agent as Player;
        ball = player.Ball.transform.position;
    }

    public override void Execute()
    {
        if (Vector3.Distance(ball, player.transform.position) > 1.5f)
        {
            Result = Result.Failure;
            return;
        }

        if (!IsBlocked(player.OpponentGoal.transform.position))
        {
            Context = player.OpponentGoal;
        }
        else if (player.TeamMates.Count > 0)
        {
            Context = FindTeamMate();
        }
        
        Result = Result.Success;
    }

    protected override void OnExit()
    {
    }

    protected override void ResetLeaf()
    {
        ball = default;
        player = null;
    }

    private bool IsBlocked(Vector3 target)
    {
        var dir = target - ball;
        var hits = Physics.RaycastAll(ball, dir, dir.magnitude);

        foreach (var hit in hits)
        {
            if (hit.transform.TryGetComponent<Player>(out var found) && found.Side != player.Side)
            {
                Debug.DrawLine(player.transform.position, target, Color.black, 2f);
                return true;
            }
        }

        return false;
    }

    private Player FindTeamMate()
    {
        foreach (var player in player.TeamMates)
        {
            if (IsBlocked(player.transform.position)) { continue; }

            return player;
        }

        return null;
    }
}
